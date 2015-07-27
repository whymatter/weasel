using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using weasel.Core;

namespace weasel {
    public class WeaselProxyBuilder<TTarget> {
        private List<IProxyScope> _scopeChain;

        public WeaselProxyBuilder<TTarget> ChainInterceptor(IWeaselInterceptor interceptor) {}

        public WeaselProxyBuilder<TTarget> ChainInterceptor(IWeaselInterceptor interceptor,
            Expression<Action<TTarget>> scope) {}

        public WeaselProxyBuilder<TTarget> ChainInterceptor<TResult>(IWeaselInterceptor interceptor,
            Expression<Func<TTarget, TResult>> scope) {}

        public TTarget BuildProxy() {}
    }
}