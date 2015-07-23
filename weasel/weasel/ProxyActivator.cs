using System;

namespace weasel {
    public class ProxyActivator {
        public TTarget CreateInstance<TTarget>(Type proxyType) {
            return (TTarget) Activator.CreateInstance(proxyType, false);
        }
    }
}