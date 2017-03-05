using System;
using System.Reflection;
using System.Reflection.Emit;
using weasel.Core;

namespace weasel.Generator {
    /// <summary>
    ///     Defines a new private Field.
    /// </summary>
    internal class PrivateFieldGenerator : IFieldAssembler {
        private readonly ITypeNameCreator _nameCreator;

        /// <summary>
        ///     Creates a new <c>PrivateFieldAssembler</c>.
        /// </summary>
        /// <param name="nameCreator">An implementation of a <c>ITypeNameCreator</c>.</param>
        public PrivateFieldGenerator(ITypeNameCreator nameCreator) {
            _nameCreator = nameCreator;
        }

        /// <summary>
        ///     Defines a new private Field.
        /// </summary>
        /// <param name="proxyClassBuilder">The <c>TypeBuilder</c> for the proxy class.</param>
        /// <param name="typeOfField">The type of the new Field.</param>
        /// <returns></returns>
        public FieldBuilder DefineField(TypeBuilder proxyClassBuilder, Type typeOfField) {
            var fieldName = _nameCreator.CreateNewFieldName(typeOfField);
            return proxyClassBuilder.DefineField(fieldName, typeOfField, GetAttributes());
        }

        /// <summary>
        ///     Returns the attributes for the private field.
        /// </summary>
        /// <returns></returns>
        private static FieldAttributes GetAttributes() => FieldAttributes.Private;
    }
}