using System;
using System.Reflection;
using System.Reflection.Emit;
using weasel.Core;

namespace weasel.Assembler {
    /// <summary>
    ///     Defines a new private Field.
    /// </summary>
    internal class PrivateFieldAssembler : IFieldAssembler {
        private readonly ITypeNameCreator _nameCreator;

        /// <summary>
        ///     Creates a new <c>PrivateFieldAssembler</c>.
        /// </summary>
        /// <param name="nameCreator">An implementation of a <c>ITypeNameCreator</c>.</param>
        public PrivateFieldAssembler(ITypeNameCreator nameCreator) {
            _nameCreator = nameCreator;
        }

        /// <summary>
        ///     Defines a new private Field.
        /// </summary>
        /// <param name="proxyClassBuilder">The <c>TypeBuilder</c> for the proxy class.</param>
        /// <param name="typeOfField">The type of the new Field.</param>
        /// <returns></returns>
        public FieldBuilder DefineField(TypeBuilder proxyClassBuilder, Type typeOfField) {
            return proxyClassBuilder.DefineField(_nameCreator.CreateNewFieldName(typeOfField), typeOfField, FieldAttributes.Private);
        }
    }
}