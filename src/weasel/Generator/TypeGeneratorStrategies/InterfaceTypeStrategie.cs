using System;
using System.Reflection;
using System.Reflection.Emit;
using weasel.Core;
using weasel.Core.Generator;

namespace weasel.Generator.TypeGeneratorStrategies {
    /// <summary>
    ///     Assembles a new type implementing the defined interface.
    /// </summary>
    internal class InterfaceTypeStrategie : ITypeGeneratorStrategie {
        private readonly ITypeNameCreator _typeNameCreator;

        /// <summary>
        ///     Creates a new <c>InterfaceTypeStrategie</c>.
        /// </summary>
        /// <param name="typeNameCreator">An instance of a <c>ITypeNameCreator</c>.</param>
        public InterfaceTypeStrategie(ITypeNameCreator typeNameCreator) {
            _typeNameCreator = typeNameCreator;
        }

        /// <summary>
        ///     Assembles a new type based on an interface.
        /// </summary>
        /// <param name="typeToWrap">The interface as type which should be wrapped.</param>
        /// <param name="moduleBuilder">The <see cref="ModuleBuilder"/> to use.</param>
        /// <returns></returns>
        public TypeBuilder CreateType(Type typeToWrap, ModuleBuilder moduleBuilder) {
            var typeName = _typeNameCreator.CreateNewTypeName(typeToWrap);
            return moduleBuilder.DefineType(typeName, GetTypeAttributes(), null, new[] {typeToWrap});
        }

        /// <summary>
        ///     Returns the <c>TypeAttributes</c> for the new proxy class.
        /// </summary>
        /// <returns>TypeAttributes</returns>
        private TypeAttributes GetTypeAttributes() => TypeAttributes.Class | TypeAttributes.Public;
    }
}