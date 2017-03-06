using System;
using System.Reflection.Emit;
using weasel.Core;
using weasel.Core.Generator;
using weasel.Generator.TypeGeneratorStrategies;

namespace weasel.Generator {
    /// <summary>
    ///     Is used to generate a new Type warpping the TargetType.
    /// </summary>
    internal class TypeGenerator : ITypeGenerator {
        private readonly ITypeNameCreator _typeNameCreator;

        /// <summary>
        ///     Creates a new TypeGenerator.
        /// </summary>
        /// <param name="typeNameCreator">An instance of a ITypeNameCreator.</param>
        public TypeGenerator(ITypeNameCreator typeNameCreator) {
            _typeNameCreator = typeNameCreator;
        }

        /// <summary>
        ///     Creates a new type which is wrapping the <param name="typeToWrap"></param>.
        /// </summary>
        /// <param name="typeToWrap">The Type for which the wrapping Type should be created.</param>
        /// <param name="moduleBuilder">The <see cref="ModuleBuilder"/> to build the wrapping type.</param>
        /// <returns></returns>
        public TypeBuilder GenerateWrappingType(Type typeToWrap, ModuleBuilder moduleBuilder) {
            var typeAssembler = GetTypeAssembler(typeToWrap);
            var proxyBuilder = typeAssembler.CreateType(typeToWrap, moduleBuilder);

            return proxyBuilder;
        }

        /// <summary>
        ///     Returns the needed <c>ITypeGeneratorStrategie</c> instance.
        /// </summary>
        /// <param name="typeToWrap">The type to for which the proxy class is needed.</param>
        /// <returns>ITypeGeneratorStrategie</returns>
        private ITypeGeneratorStrategie GetTypeAssembler(Type typeToWrap) {
            if (typeToWrap.IsInterface) {
                return new InterfaceTypeStrategie(_typeNameCreator);
            }

            return new ClassTypeStrategie(_typeNameCreator);
        }
    }
}