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

        public void CreateConstructor(TypeBuilder proxyClassBuilder, List<ConstructorInfo> constructorInfos,
            List<Type> interceptorsForConstructor) {
            var privateFields =
                interceptorsForConstructor.Select(constructorType => _privateFieldAssembler.DefineField(proxyClassBuilder, constructorType));

            foreach (var constructorInfo in constructorInfos) {
                var baseConstructorTypes = GetConstructorTypes(constructorInfo);
                var combineTypes = CombineTypes(baseConstructorTypes, interceptorsForConstructor);
                combineTypes = new Type[0];
                var constructorBuilder =
                    proxyClassBuilder.DefineConstructor(GetMethodAttributes(), CallingConventions.HasThis, combineTypes);

                var constructorIlGenerator = constructorBuilder.GetILGenerator();

                for (var i = 1; i <= baseConstructorTypes.Count; i++)
                {
                    constructorIlGenerator.Emit(OpCodes.Ldarg, i);
                }

                constructorIlGenerator.Emit(OpCodes.Call, constructorInfo);
                constructorIlGenerator.Emit(OpCodes.Ret);
            }
        }

        internal List<Type> GetConstructorTypes(ConstructorInfo constructorInfo) {
            return constructorInfo.GetParameters().Select(parameter => parameter.ParameterType).ToList();
        }

        internal Type[] CombineTypes(List<Type> baseConstructorTypes, List<Type> interceptorsForConstructor) {
            return baseConstructorTypes.Union(interceptorsForConstructor).ToArray();
        }

        internal MethodAttributes GetMethodAttributes() {
            return MethodAttributes.Public | MethodAttributes.HideBySig;
        }
    }
}