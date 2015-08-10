using System;
using System.Collections.Generic;
using System.Linq;
using weasel.Assembler;
using weasel.Core;
using weasel.Core.Assembler;

namespace weasel {
    /// <summary>
    ///     Is used to generate a new Type warpping the TargetType.
    /// </summary>
    internal class TypeGenerator {
        private readonly IModulBuilderGenerator _modulBuilderGenerator;
        private readonly ITypeNameCreator _typeNameCreator;

        /// <summary>
        ///     Creates a new TypeGenerator.
        /// </summary>
        /// <param name="modulBuilderGenerator">An instance of a IModulBuilderGenerator.</param>
        /// <param name="typeNameCreator">An instance of a ITypeNameCreator.</param>
        public TypeGenerator(IModulBuilderGenerator modulBuilderGenerator, ITypeNameCreator typeNameCreator) {
            _modulBuilderGenerator = modulBuilderGenerator;
            _typeNameCreator = typeNameCreator;
        }

        public void GenerateWrappingType(Type typeToWrap, List<ProxyLevel> proxyLevels) {
            var typeAssembler = GetTypeAssembler(typeToWrap);
            var proxyClass = typeAssembler.CreateType(typeToWrap);

            
        }

        /// <summary>
        ///     Returns the needed <c>ITypeAssembler</c> instance.
        /// </summary>
        /// <param name="typeToWrap">The type to for which the proxy class is needed.</param>
        /// <returns>ITypeAssembler</returns>
        internal ITypeAssembler GetTypeAssembler(Type typeToWrap) {
            if (typeToWrap.IsInterface) {
                return new InterfaceImplementationAssembler(_typeNameCreator, _modulBuilderGenerator);
            }

            return new BaseClassImplementationAssembler(_typeNameCreator, _modulBuilderGenerator);
        }
    }
}