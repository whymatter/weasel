using System;
using System.Reflection.Emit;

namespace weasel.Core.Generator {
    /// <summary>
    ///     Used to create Fields.
    /// </summary>
    internal interface IFieldGenerator {
        /// <summary>
        ///     Defines a new private Field.
        /// </summary>
        /// <param name="proxyClassBuilder">The <c>TypeBuilder</c> for the proxy class.</param>
        /// <param name="typeOfField">The type of the new Field.</param>
        /// <returns></returns>
        FieldBuilder DefineField(TypeBuilder proxyClassBuilder, Type typeOfField);
    }
}