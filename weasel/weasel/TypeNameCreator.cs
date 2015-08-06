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
            return string.Format("{0}_DYNAMIC_{1}", typeToWrap.FullName, _timestampProvider.GetTimestampFromNow());
        }
    }
}