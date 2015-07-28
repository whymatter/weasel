using weasel.Core;

namespace weasel {
    internal class ProxyLevel {
        private IProxyScope _proxyScope;
        private IWeaselInterceptor _weaselInterceptor;

        public ProxyLevel(IWeaselInterceptor weaselInterceptor, IProxyScope proxyScope) {
            _weaselInterceptor = weaselInterceptor;
            _proxyScope = proxyScope;
        }
    }
}