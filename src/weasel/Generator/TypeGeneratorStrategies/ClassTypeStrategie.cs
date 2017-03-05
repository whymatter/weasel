using System;
using System.Reflection;
using System.Reflection.Emit;
using weasel.Core;
using weasel.Core.Assembler;

namespace weasel.Generator.TypeGeneratorStrategies {
    /// <summary>
    ///     Assembles a new type, derived from the type passsed in the CreateType function.
    /// </summary>
    internal class ClassTypeStrategie : ITypeGeneratorStrategie {
        private readonly ITypeNameCreator _typeNameCreator;

        /// <summary>
        ///     Creates a new <c>ClassTypeStrategie</c>.
        /// </summary>
        /// <param name="typeNameCreator"></param>
        public ClassTypeStrategie(ITypeNameCreator typeNameCreator) {
            _typeNameCreator = typeNameCreator;
        }

        /// <summary>
        ///     Assembles a new type derived from the <param name="typeToWrap"></param> Type.
        /// </summary>
        /// <param name="typeToWrap">The interface as type which should be wrapped.</param>
        /// <param name="moduleBuilder">The <see cref="ModuleBuilder"/> to use.</param>
        /// <returns></returns>
        public TypeBuilder CreateType(Type typeToWrap, ModuleBuilder moduleBuilder) {
            var typeName = _typeNameCreator.CreateNewTypeName(typeToWrap);
            return moduleBuilder.DefineType(typeName, GetTypeAttributes(), typeToWrap);
        }

        /// <summary>
        ///     Returns the <see cref="TypeAttributes"/> for the new proxy class.
        /// </summary>
        /// <returns>TypeAttributes</returns>
        private TypeAttributes GetTypeAttributes() => TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.BeforeFieldInit;
    }
}