using System;
using weasel.Internal.Core;

namespace weasel.Internal {
    internal class TypeNameCreator : ITypeNameCreator {
        private readonly ITimestampProvider _timestampProvider;

        /// <summary>
        ///     Creates a new TypeNameCreator.
        /// </summary>
        /// <param name="timestampProvider"></param>
        public TypeNameCreator(ITimestampProvider timestampProvider) {
            _timestampProvider = timestampProvider;
        }

        /// <summary>
        ///     Creates a the name for the proxy class, proxing the typeToWrap.
        /// </summary>
        /// <param name="typeToWrap">The type to wrap with an proxy.</param>
        /// <returns></returns>
        public string CreateNewTypeName(Type typeToWrap) {
            return $"{typeToWrap.Name}_DYNAMIC_{_timestampProvider.GetTimestampFromNow()}";
        }

        /// <summary>
        ///     Creats a new name for a field in the proxy class.
        /// </summary>
        /// <param name="typeForField">The type of the field.</param>
        /// <returns></returns>
        public string CreateNewFieldName(Type typeForField) {
            return $"{typeForField.FullName}_{_timestampProvider.GetTimestampFromNow()}";
        }
    }
}