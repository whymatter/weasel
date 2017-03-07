using System;

namespace weasel.Internal.Core {
    internal interface ITypeNameCreator {
        /// <summary>
        ///     Creates a new name for the proxy class, proxing the typeToWrap.
        /// </summary>
        /// <param name="typeToWrap">The type to wrap with an proxy.</param>
        /// <returns></returns>
        string CreateNewTypeName(Type typeToWrap);

        /// <summary>
        ///     Creats a new name for a field in the proxy class.
        /// </summary>
        /// <param name="typeForField">The type of the field.</param>
        /// <returns></returns>
        string CreateNewFieldName(Type typeForField);
    }
}