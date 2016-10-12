using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using weasel.Core;

namespace weasel.Assembler {
    /// <summary>
    ///     Assembles the constructors for the proxy and calls all base constructor.
    /// </summary>
    internal class BaseClassConstructorAssembler {
        private readonly IModulBuilderGenerator _modulBuilderGenerator;
        private readonly PrivateFieldAssembler _privateFieldAssembler;

        public BaseClassConstructorAssembler(IModulBuilderGenerator modulBuilderGenerator, PrivateFieldAssembler privateFieldAssembler) {
            _modulBuilderGenerator = modulBuilderGenerator;
            _privateFieldAssembler = privateFieldAssembler;
        }

        /// <summary>
        ///     Creates all constructors needed.
        /// </summary>
        /// <param name="proxyClassBuilder">The <c>TypeBuilder</c> for the proxy class.</param>
        /// <param name="constructorInfos">All existing constructors on the class to proxy.</param>
        /// <param name="amountOfInterceptors">The Amount of <c>IWeaselInterceptor</c> needed.</param>
        public void CreateConstructor(TypeBuilder proxyClassBuilder, List<ConstructorInfo> constructorInfos,
            int amountOfInterceptors) {
            var privateFields = new List<FieldBuilder>(amountOfInterceptors);

            for (var i = 0; i < amountOfInterceptors; i++) {
                privateFields.Add(_privateFieldAssembler.DefineField(proxyClassBuilder, typeof (IWeaselInterceptor)));
            }

            foreach (var constructorInfo in constructorInfos) {
                var baseConstructorTypes = GetConstructorTypes(constructorInfo);
                var combineTypes = CombineTypes(baseConstructorTypes, amountOfInterceptors);
                var constructorBuilder =
                    proxyClassBuilder.DefineConstructor(GetMethodAttributes(), CallingConventions.HasThis, combineTypes);

                var constructorIlGenerator = constructorBuilder.GetILGenerator();

                // We have to load the 'this' argument from index 0 first
                constructorIlGenerator.Emit(OpCodes.Ldarg_0);
                
                for (var i = 0; i < baseConstructorTypes.Count; i++) {
                    constructorIlGenerator.Emit(OpCodes.Ldarg, i + 1);
                }

                constructorIlGenerator.Emit(OpCodes.Call, constructorInfo);

                for (var i = 0; i < amountOfInterceptors; i++) {
                    var methodArgumentIndex = baseConstructorTypes.Count + 1 + i;
                    // We have to load the 'this' argument from index 0 first
                    constructorIlGenerator.Emit(OpCodes.Ldarg_0);
                    constructorIlGenerator.Emit(OpCodes.Ldarg, methodArgumentIndex);
                    constructorIlGenerator.Emit(OpCodes.Stfld, privateFields[i]);
                }

                constructorIlGenerator.Emit(OpCodes.Ret);
            }
        }

        internal List<Type> GetConstructorTypes(ConstructorInfo constructorInfo) {
            return constructorInfo.GetParameters().Select(parameter => parameter.ParameterType).ToList();
        }

        internal Type[] CombineTypes(List<Type> baseConstructorTypes, int amountOfInterceptors) {
            return baseConstructorTypes.Concat(Enumerable.Repeat(typeof (IWeaselInterceptor), amountOfInterceptors)).ToArray();
        }

        internal MethodAttributes GetMethodAttributes() {
            return MethodAttributes.Public | MethodAttributes.HideBySig;
        }
    }
}