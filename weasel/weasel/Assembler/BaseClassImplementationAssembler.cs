using System;
using System.Reflection;
using System.Reflection.Emit;
using weasel.Core;
using weasel.Core.Assembler;

namespace weasel.Assembler {
    /// <summary>
    ///     Assembles a new type, derived from the type passsed in the CreateType function.
    /// </summary>
    internal class BaseClassImplementationAssembler : ITypeAssembler {
        private readonly IModulBuilderGenerator _modulBuilderGenerator;
        private readonly ITypeNameCreator _typeNameCreator;

        /// <summary>
        ///     Creates a new <c>BaseClassImplementationAssembler</c>.
        /// </summary>
        /// <param name="typeNameCreator"></param>
        /// <param name="modulBuilderGenerator"></param>
        public BaseClassImplementationAssembler(ITypeNameCreator typeNameCreator, IModulBuilderGenerator modulBuilderGenerator) {
            _typeNameCreator = typeNameCreator;
            _modulBuilderGenerator = modulBuilderGenerator;
        }

        /// <summary>
        ///     Assembles a new wrapping type for the baseClassType.
        ///     The new type will derive from baseClassType.
        /// </summary>
        /// <param name="baseClassType">The base-class which should be wrapped.</param>
        /// <returns></returns>
        public TypeBuilder CreateType(Type baseClassType) {
            var typeName = _typeNameCreator.CreateNewTypeName(baseClassType);
            return _modulBuilderGenerator.ModuleBuilder.DefineType(typeName, GetTypeAttributes(), baseClassType);
        }

        /// <summary>
        ///     Returns the <c>TypeAttributes</c> for the new proxy class.
        /// </summary>
        /// <returns>TypeAttributes</returns>
        private TypeAttributes GetTypeAttributes() {
            return TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.BeforeFieldInit;
        }
    }
}