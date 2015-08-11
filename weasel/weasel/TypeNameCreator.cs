using System;
using weasel.Core;

namespace weasel {
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
            return string.Format("{0}_DYNAMIC_{1}", typeToWrap.Name, _timestampProvider.GetTimestampFromNow());
        }

        /// <summary>
        ///     Creats a new name for a field in the proxy class.
        /// </summary>
        /// <param name="typeForField">The type of the field.</param>
        /// <returns></returns>
        public string CreateNewFieldName(Type typeForField)
        {
            return string.Format("{0}_{1}", typeForField.FullName, _timestampProvider.GetTimestampFromNow());
        }
    }
}