using System;
using System.Linq.Expressions;
using weasel.Core;
using weasel.Core.Exceptions;
using weasel.Interceptors;
using weasel.Scopes;

namespace weasel {
    public class WeaselProxyChainBuilder<TTarget> : IWeaselProxyChainBuilder<TTarget> where TTarget : class {
        /// <summary>
        ///     The store for the interceptors added by the ChainInterceptor methods
        /// </summary>
        private readonly ProxyChain _proxyChain;

        public WeaselProxyChainBuilder() {
            _proxyChain = new ProxyChain();
        }

        /// <summary>
        ///     Adds a new <c>IWeaselInterceptor</c> to the interceptor chain.
        ///     The GlobalScope is used for this ProxyLevel.
        /// </summary>
        /// <param name="interceptor">The IWeaselInterceptor which should be added to the chain.</param>
        /// <returns></returns>
        public WeaselProxyChainBuilder<TTarget> ChainInterceptor(IWeaselInterceptor interceptor) {
            // Add a new ProxyLevel with the GlobalSope to the chain
            _proxyChain.PushProxyLevel(interceptor, new GlobalScope());
            return this;
        }

        public WeaselProxyChainBuilder<TTarget> ChainInterceptor<TResult>(Action interceptor, Expression<Action<TTarget>> scope) {
            throw new NotImplementedException();
        }

        public WeaselProxyChainBuilder<TTarget> ChainInterceptor<TResult>(Action<TTarget> interceptor, Expression<Action<TTarget>> scope) {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Adds a new interceptor within the defined MethodScope to the interceptor chain.
        ///     If the interceptor executes the <c>Action</c> get´s called.
        /// </summary>
        /// <param name="interceptor">The action to execute if the interceptor get´s called.</param>
        /// <param name="scope">The MethodScope for the interceptor.</param>
        /// <returns></returns>
        public WeaselProxyChainBuilder<TTarget> ChainInterceptor(Action interceptor, Expression<Action<TTarget>> scope) {
            return ChainInterceptor(new ActionWithoutParameterInterceptor(interceptor), scope);
        }

        /// <summary>
        ///     Adds a new interceptor within the defined MethodScope to the interceptor chain.
        ///     If the interceptor executes the <c>Action</c> get´s called.
        /// </summary>
        /// <param name="interceptor">The action to execute if the interceptor get´s called.</param>
        /// <param name="scope">The MethodScope for the interceptor.</param>
        /// <returns></returns>
        public WeaselProxyChainBuilder<TTarget> ChainInterceptor(Action<TTarget> interceptor, Expression<Action<TTarget>> scope) {
            return ChainInterceptor(new ActionWithTargetParameterInterceptor<TTarget>(interceptor), scope);
        }

        /// <summary>
        ///     Adds a new <c>IWeaselInterceptor</c> with the defined MethodScope to the interceptor chain.
        /// </summary>
        /// <param name="interceptor">The IWeaselInterceptor which should be added to the chain.</param>
        /// <param name="scope">The MethodScope for the interceptor.</param>
        /// <returns></returns>
        public WeaselProxyChainBuilder<TTarget> ChainInterceptor(IWeaselInterceptor interceptor, Expression<Action<TTarget>> scope) {
            if (scope == null) {
                throw new InvalidScopeExpressionException("scope");
            }

            var methodExpression = scope.Body as MethodCallExpression;

            if (methodExpression == null) {
                throw new InvalidScopeExpressionException("scope");
            }

            _proxyChain.PushProxyLevel(interceptor, GetMethodScope(methodExpression));
            return this;
        }

        /// <summary>
        ///     Adds a new interceptor within the defined MethodScope to the interceptor chain.
        ///     If the interceptor executes the <c>Action</c> get´s called.
        /// </summary>
        /// <typeparam name="TResult">The return type of the scope function.</typeparam>
        /// <param name="interceptor">The action to execute if the interceptor get´s called.</param>
        /// <param name="scope">The Function or PropertyScope for the interceptor.</param>
        /// <returns></returns>
        public WeaselProxyChainBuilder<TTarget> ChainInterceptor<TResult>(Action<TTarget> interceptor, Expression<Func<TTarget, TResult>> scope)
        {
            return ChainInterceptor(new ActionWithTargetParameterInterceptor<TTarget>(interceptor), scope);
        }

        /// <summary>
        ///     Adds a new interceptor within the defined MethodScope to the interceptor chain.
        ///     If the interceptor executes the <c>Action</c> get´s called.
        /// </summary>
        /// <typeparam name="TResult">The return type of the scope function.</typeparam>
        /// <param name="interceptor">The action to execute if the interceptor get´s called.</param>
        /// <param name="scope">The Function or PropertyScope for the interceptor.</param>
        /// <returns></returns>
        public WeaselProxyChainBuilder<TTarget> ChainInterceptor<TResult>(Action interceptor, Expression<Func<TTarget, TResult>> scope)
        {
            return ChainInterceptor(new ActionWithoutParameterInterceptor(interceptor), scope);
        }

        /// <summary>
        ///     Adds a new <c>IWeaselInterceptor</c> with the defined Function or PropertyScope to the interceptor chain.
        /// </summary>
        /// <typeparam name="TResult">The return type for the function scope.</typeparam>
        /// <param name="interceptor">The IWeaselInterceptor which should be added to the chain.</param>
        /// <param name="scope">The Function or PropertyScope for the interceptor.</param>
        /// <returns></returns>
        public WeaselProxyChainBuilder<TTarget> ChainInterceptor<TResult>(IWeaselInterceptor interceptor, Expression<Func<TTarget, TResult>> scope) {
            if (interceptor == null) {
                throw new ArgumentNullException("interceptor");
            }

            if (scope == null) {
                throw new InvalidScopeExpressionException("scope");
            }

            IProxyScope proxyScope = null;

            var methodExpression = scope.Body as MethodCallExpression;
            var propertyExpression = scope.Body as MemberExpression;

            // If it´s a MethodCallExpression then we need a MethodScope
            if (methodExpression != null) {
                proxyScope = GetMethodScope(methodExpression);
            }

            // Otherways if it´s a MemberExpression then we need a PropertyScope
            if (propertyExpression != null) {
                proxyScope = new PropertyScope(propertyExpression.Member);
            }

            // If proxyScope is still null then the input have to be invalid
            if (proxyScope == null) {
                throw new InvalidScopeExpressionException("scope");
            }

            _proxyChain.PushProxyLevel(new ProxyLevel(interceptor, proxyScope));
            return this;
        }

        public TTarget BuildProxy() {
            throw new NotImplementedException();
        }


        /// <summary>
        ///     Returns the <c>MethodScope</c> for the passed <c>MethodCallExpression</c>
        /// </summary>
        /// <param name="methodExpression"></param>
        /// <returns></returns>
        private MethodScope GetMethodScope(MethodCallExpression methodExpression) {
            return new MethodScope(methodExpression.Method, methodExpression.Arguments);
        }
    }
}