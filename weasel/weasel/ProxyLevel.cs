using System;
using weasel.Core;

namespace weasel {
    internal class ProxyLevel {
        private IProxyScope _proxyScope;
        private IWeaselInterceptor _weaselInterceptor;

        public ProxyLevel(IWeaselInterceptor weaselInterceptor, IProxyScope proxyScope) {
            if (weaselInterceptor == null) {
                throw new ArgumentNullException("weaselInterceptor");
            }

            if (proxyScope == null) {
                throw new ArgumentNullException("proxyScope");
            }

            _weaselInterceptor = weaselInterceptor;
            _proxyScope = proxyScope;
        }
    }
}