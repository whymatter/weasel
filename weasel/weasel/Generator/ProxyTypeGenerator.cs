using System;
using System.Reflection.Emit;
using weasel.Assembler;
using weasel.Core;
using weasel.Core.Assembler;

namespace weasel {
    /// <summary>
    ///     Is used to generate a new Type warpping the TargetType.
    /// </summary>
    internal class ProxyTypeGenerator {
        private readonly IModuleGenerator _moduleGenerator;
        private readonly ITypeNameCreator _typeNameCreator;

        /// <summary>
        ///     Creates a new TypeGenerator.
        /// </summary>
        /// <param name="moduleGenerator">An instance of a IModulBuilderGenerator.</param>
        /// <param name="typeNameCreator">An instance of a ITypeNameCreator.</param>
        public ProxyTypeGenerator(IModuleGenerator moduleGenerator, ITypeNameCreator typeNameCreator) {
            _moduleGenerator = moduleGenerator;
            _typeNameCreator = typeNameCreator;
        }

        /// <summary>
        ///     Creates a new type which is wrapping the <param name="typeToWrap"></param>.
        /// </summary>
        /// <param name="typeToWrap">The Type for which the wrapping Type should be created.</param>
        /// <returns></returns>
        public TypeBuilder GenerateWrappingType(Type typeToWrap) {
            var typeAssembler = GetTypeAssembler(typeToWrap);
            var proxyBuilder = typeAssembler.CreateType(typeToWrap);

            return proxyBuilder;
        }

        /// <summary>
        ///     Creates a new type which is wrapping the
        ///     <typeparam name="TType"></typeparam>
        ///     .
        /// </summary>
        /// <typeparam name="TType">Type to wrap.</typeparam>
        /// <returns></returns>
        public TypeBuilder GenerateWrappingType<TType>() where TType : class => GenerateWrappingType(typeof(TType));

        /// <summary>
        ///     Returns the needed <c>ITypeAssembler</c> instance.
        /// </summary>
        /// <param name="typeToWrap">The type to for which the proxy class is needed.</param>
        /// <returns>ITypeAssembler</returns>
        private ITypeAssembler GetTypeAssembler(Type typeToWrap) {
            if (typeToWrap.IsInterface) {
                return new InterfaceImplementationAssembler(_typeNameCreator, _moduleGenerator);
            }

            return new BaseClassImplementationAssembler(_typeNameCreator, _moduleGenerator);
        }
    }
}