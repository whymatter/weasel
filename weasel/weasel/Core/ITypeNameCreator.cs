using System;

namespace weasel.Core {
    internal interface ITypeNameCreator {
        /// <summary>
        ///     Creates a new name for the proxy class, proxing the typeToWrap.
        /// </summary>
        /// <param name="typeToWrap">The type to wrap with an proxy.</param>
        /// <returns></returns>
        string CreateNewTypeName(Type typeToWrap);
    }
}