using System;
using System.Reflection;
using System.Reflection.Emit;
using weasel.Core;
using weasel.Core.Assembler;

namespace weasel.Assembler {
    /// <summary>
    ///     Assembles a new type implementing the defined interface.
    /// </summary>
    internal class InterfaceImplementationAssembler : ITypeAssembler {
        private readonly IModulBuilderGenerator _modulBuilderGenerator;
        private readonly ITypeNameCreator _typeNameCreator;

        /// <summary>
        ///     Creates a new <c>InterfaceImplementationAssembler</c>.
        /// </summary>
        /// <param name="typeNameCreator">An instance of a <c>ITypeNameCreator</c>.</param>
        /// <param name="modulBuilderGenerator">An instance of a <c>IModulBuilderGenerator</c>.</param>
        public InterfaceImplementationAssembler(ITypeNameCreator typeNameCreator, IModulBuilderGenerator modulBuilderGenerator) {
            _typeNameCreator = typeNameCreator;
            _modulBuilderGenerator = modulBuilderGenerator;
        }

        /// <summary>
        ///     Assembles a new type, implementing the interface defined in typeToWrap.
        /// </summary>
        /// <param name="interfaceForWrapping">The interface as type which should be wrapped.</param>
        /// <returns></returns>
        public TypeBuilder CreateType(Type interfaceForWrapping) {
            var typeName = _typeNameCreator.CreateNewTypeName(interfaceForWrapping);
            return _modulBuilderGenerator.ModuleBuilder.DefineType(typeName, GetTypeAttributes(), null, new[] { interfaceForWrapping });
        }

        /// <summary>
        ///     Returns the <c>TypeAttributes</c> for the new proxy class.
        /// </summary>
        /// <returns>TypeAttributes</returns>
        private TypeAttributes GetTypeAttributes() {
            return TypeAttributes.Class | TypeAttributes.Public;
        }
    }
}