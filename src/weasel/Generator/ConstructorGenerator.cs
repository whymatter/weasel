using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using weasel.Core.Generator;

namespace weasel.Generator {
    /// <summary>
    ///     Assembles the constructors for the proxy and calls all base constructor.
    /// </summary>
    internal class ConstructorGenerator : IConstructorGenerator {
        /// <summary>
        ///     Creates all constructors needed.
        /// </summary>
        /// <param name="typeBuilder">The <see cref="TypeBuilder"/> for the proxy class.</param>
        /// <param name="constructorInfos">All existing <see cref="ConstructorInfo"/>'s on the class to proxy.</param>
        /// <param name="interceptors">The fields of all interceptors.</param>
        public void CreateConstructor(TypeBuilder typeBuilder, List<ConstructorInfo> constructorInfos,
            List<FieldBuilder> interceptors) {
            if (constructorInfos.Any()) {
                foreach (var constructorInfo in constructorInfos) {
                    var constructorParameterTypes = GetConstructorParameterTypes(constructorInfo);
                    var parametersWithInterceptors = AddInterceptorTypes(constructorParameterTypes, interceptors);

                    var constructorBuilder = DefineConstructor(typeBuilder, parametersWithInterceptors);
                    var constructorIlGenerator = constructorBuilder.GetILGenerator();

                    CreateBaseClassCall(constructorIlGenerator, constructorParameterTypes, constructorInfo);

                    CreateInterceptorAssignBlock(constructorParameterTypes.Count, constructorIlGenerator, interceptors);

                    constructorIlGenerator.Emit(OpCodes.Ret);
                }
            }
            else {
                var constructorParams = AddInterceptorTypes(new Type[0], interceptors);
                var constructorBuilder = DefineConstructor(typeBuilder, constructorParams);
                var constructorIlGenerator = constructorBuilder.GetILGenerator();
                CreateInterceptorAssignBlock(0, constructorIlGenerator, interceptors);

                constructorIlGenerator.Emit(OpCodes.Ret);
            }
        }

        /// <summary>
        /// Assigns all interceptors passed into the constructor to their private fields.
        /// </summary>
        /// <param name="offset">Amount of other parameters before the interceptors.</param>
        /// <param name="constructorIlGenerator">The <see cref="ILGenerator"/> for the constructor.</param>
        /// <param name="privateFields">All <see cref="FieldBuilder"/> for the interceptors.</param>
        private void CreateInterceptorAssignBlock(int offset, ILGenerator constructorIlGenerator,
            IReadOnlyList<FieldBuilder> privateFields) {
            for (var i = 0; i < privateFields.Count; i++) {
                var methodArgumentIndex = offset + 1 + i;

                // We have to load the 'this' argument from index 0 first
                constructorIlGenerator.Emit(OpCodes.Ldarg_0);

                // Load interceptor
                constructorIlGenerator.Emit(OpCodes.Ldarg, methodArgumentIndex);

                // Assign to field
                constructorIlGenerator.Emit(OpCodes.Stfld, privateFields[i]);
            }
        }

        private ConstructorBuilder DefineConstructor(TypeBuilder typeBuilder, Type[] parameterTypes) {
            return typeBuilder
                .DefineConstructor(
                    GetConstructorMethodAttributes(),
                    GetConstructorCallingConvention(),
                    parameterTypes);
        }

        private void CreateBaseClassCall(ILGenerator constructorIlGenerator, List<Type> constructorParameterTypes,
            ConstructorInfo constructorInfo) {
            // Load 'this' to stack
            constructorIlGenerator.Emit(OpCodes.Ldarg_0);

            // Load every constructor parameter to stack
            for (var i = 0; i < constructorParameterTypes.Count; i++) {
                constructorIlGenerator.Emit(OpCodes.Ldarg, i + 1);
            }

            // Call base constructor
            constructorIlGenerator.Emit(OpCodes.Call, constructorInfo);
        }

        private List<Type> GetConstructorParameterTypes(ConstructorInfo constructorInfo) {
            return constructorInfo.GetParameters().Select(parameter => parameter.ParameterType).ToList();
        }

        private Type[] AddInterceptorTypes(IEnumerable<Type> baseConstructorTypes, List<FieldBuilder> interceptors) {
            return baseConstructorTypes.Concat(interceptors.Select(i => i.FieldType)).ToArray();
        }

        private MethodAttributes GetConstructorMethodAttributes()
            => MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.RTSpecialName | MethodAttributes.SpecialName;

        private static CallingConventions GetConstructorCallingConvention() => CallingConventions.HasThis;
    }
}