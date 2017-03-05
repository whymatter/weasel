using System;
using System.Linq.Expressions;

namespace weasel.Core {
    public interface IWeaselProxyChainBuilder<TTarget> where TTarget : class {
        /// <summary>
        ///     Adds a new <c>IWeaselInterceptor</c> to the interceptor chain.
        ///     The GlobalScope is used for this ProxyLevel.
        /// </summary>
        /// <param name="interceptor">The IWeaselInterceptor which should be added to the chain.</param>
        /// <returns></returns>
        WeaselProxyChainBuilder<TTarget> ChainInterceptor(IWeaselInterceptor interceptor);

        /// <summary>
        ///     Adds a new interceptor within the defined MethodScope to the interceptor chain.
        ///     If the interceptor executes the <c>Action</c> get큦 called.
        /// </summary>
        /// <param name="interceptor">The action to execute if the interceptor get큦 called.</param>
        /// <param name="scope">The MethodScope for the interceptor.</param>
        /// <returns></returns>
        WeaselProxyChainBuilder<TTarget> ChainInterceptor<TResult>(Action interceptor, Expression<Action<TTarget>> scope);

        /// <summary>
        ///     Adds a new interceptor within the defined MethodScope to the interceptor chain.
        ///     If the interceptor executes the <c>Action</c> get큦 called.
        /// </summary>
        /// <param name="interceptor">The action to execute if the interceptor get큦 called.</param>
        /// <param name="scope">The MethodScope for the interceptor.</param>
        /// <returns></returns>
        WeaselProxyChainBuilder<TTarget> ChainInterceptor<TResult>(Action<TTarget> interceptor, Expression<Action<TTarget>> scope);

        /// <summary>
        ///     Adds a new <c>IWeaselInterceptor</c> within the defined MethodScope to the interceptor chain.
        /// </summary>
        /// <param name="interceptor">The IWeaselInterceptor which should be added to the chain.</param>
        /// <param name="scope">The MethodScope for the interceptor.</param>
        /// <returns></returns>
        WeaselProxyChainBuilder<TTarget> ChainInterceptor(IWeaselInterceptor interceptor, Expression<Action<TTarget>> scope);

        /// <summary>
        ///     Adds a new interceptor within the defined MethodScope to the interceptor chain.
        ///     If the interceptor executes the <c>Action</c> get큦 called.
        /// </summary>
        /// <typeparam name="TResult">The return type of the scope function.</typeparam>
        /// <param name="interceptor">The action to execute if the interceptor get큦 called.</param>
        /// <param name="scope">The Function or PropertyScope for the interceptor.</param>
        /// <returns></returns>
        WeaselProxyChainBuilder<TTarget> ChainInterceptor<TResult>(Action interceptor, Expression<Func<TTarget, TResult>> scope);

        /// <summary>
        ///     Adds a new interceptor within the defined MethodScope to the interceptor chain.
        ///     If the interceptor executes the <c>Action</c> get큦 called.
        /// </summary>
        /// <typeparam name="TResult">The return type of the scope function.</typeparam>
        /// <param name="interceptor">The action to execute if the interceptor get큦 called.</param>
        /// <param name="scope">The Function or PropertyScope for the interceptor.</param>
        /// <returns></returns>
        WeaselProxyChainBuilder<TTarget> ChainInterceptor<TResult>(Action<TTarget> interceptor, Expression<Func<TTarget, TResult>> scope);

        /// <summary>
        ///     Adds a new <c>IWeaselInterceptor</c> with the defined Function or PropertyScope to the interceptor chain.
        /// </summary>
        /// <typeparam name="TResult">The return type for the function scope.</typeparam>
        /// <param name="interceptor">The IWeaselInterceptor which should be added to the chain.</param>
        /// <param name="scope">The Function or PropertyScope for the interceptor.</param>
        /// <returns></returns>
        WeaselProxyChainBuilder<TTarget> ChainInterceptor<TResult>(IWeaselInterceptor interceptor, Expression<Func<TTarget, TResult>> scope);

        TTarget BuildProxy();
    }
}