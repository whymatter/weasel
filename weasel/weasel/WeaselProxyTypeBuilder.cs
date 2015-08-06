using System;
using weasel.Core;

namespace weasel {
    /// <summary>
    ///     Generates a <c>Type</c> for the new Proxy.
    /// </summary>
    internal class WeaselProxyTypeBuilder<TTarget> {
        private readonly IModulBuilderGenerator _modulBuilderGenerator;

        /// <summary>
        ///     Creates a new WeaselProxyTypeBuilder.
        /// </summary>
        /// <param name="modulBuilderGenerator">The instance of an IModulBuilderGenerator.</param>
        public WeaselProxyTypeBuilder(IModulBuilderGenerator modulBuilderGenerator) {
            _modulBuilderGenerator = modulBuilderGenerator;
        }

        public Type BuildProxyType(IProxyChain proxyChain) {
            var baseType = typeof (TTarget);
            return null;
        }
    }
}