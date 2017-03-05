using weasel.Core.Configuration;

namespace weasel {
    /// <summary>
    ///     His job is the creation of ProxyObjects
    /// </summary>
    public class ProxyProvider {
        public WeaselProxyChainBuilder<TTarget> CreateProxy<TTarget>(IWeaselConfiguration configuration)
            where TTarget : class {
            return new WeaselProxyChainBuilder<TTarget>();
        }
    }
}