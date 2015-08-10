using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using weasel.Core;

namespace weasel.Assembler {
    /// <summary>
    /// Assembles the constructors for the proxy and calls all base constructor.
    /// </summary>
    internal class BaseClassConstructorAssembler {
        private readonly IModulBuilderGenerator _modulBuilderGenerator;

        public BaseClassConstructorAssembler(IModulBuilderGenerator modulBuilderGenerator) {
            _modulBuilderGenerator = modulBuilderGenerator;
        }

        public void CreateConstructor(TypeBuilder proxyClassBuilder, List<ConstructorInfo> constructorInfos, List<Type> interceptorsForConstructor) {
            foreach (var constructorInfo in constructorInfos) {
                var constructorTypes = GetConstructorTypes(interceptorsForConstructor, constructorInfo);

                var constructorBuilder = proxyClassBuilder.DefineConstructor(GetMethodAttributes(), CallingConventions.HasThis,
                    constructorTypes);
            }
        }

        internal Type[] GetConstructorTypes(List<Type> interceptorsForConstructor, ConstructorInfo constructorInfo) {
            var baseClassParameters = constructorInfo.GetParameters().Select(parameter => parameter.ParameterType);
            var newConstructorTypes = baseClassParameters.Union(interceptorsForConstructor).ToArray();
            return newConstructorTypes;
        }

        internal MethodAttributes GetMethodAttributes() {
            return MethodAttributes.Public;
        }
    }
}