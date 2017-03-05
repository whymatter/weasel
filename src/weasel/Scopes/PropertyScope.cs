using System.Reflection;
using weasel.Core;

namespace weasel.Scopes {
    /// <summary>
    ///     Encapsulates the IWeaselInterceptor on a property.
    /// </summary>
    internal class PropertyScope : IProxyScope {
        /// <summary>
        ///     Creates a new PropertyScope
        /// </summary>
        /// <param name="memberInfo">The MemberInfo on which the interceptor is scoped.</param>
        public PropertyScope(MemberInfo memberInfo) {
            MemberInfo = memberInfo;
        }

        /// <summary>
        ///     The MemberInfo on which the interceptor is scoped.
        /// </summary>
        public MemberInfo MemberInfo { get; private set; }

        /// <summary>
        ///     The access method on which the interceptor is scoped.
        /// </summary>
        public PropertyAccessMethods AccessMethod { get; set; }
    }
}