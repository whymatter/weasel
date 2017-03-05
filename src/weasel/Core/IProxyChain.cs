using System.Collections.ObjectModel;

namespace weasel.Core {
    internal interface IProxyChain {
        /// <summary>
        ///     The single <c>ProxyLevel</c> steps.
        /// </summary>
        ReadOnlyCollection<ProxyLevel> ProxyLevels { get; }

        /// <summary>
        ///     Pushes a new <c>ProxyLevel</c> into the ProxyLevels.
        /// </summary>
        /// <param name="newProxyLevel">The new ProxyLevel instance.</param>
        void PushProxyLevel(ProxyLevel newProxyLevel);

        /// <summary>
        ///     Creates a new <c>ProxyLevel</c> and pushes it into the ProxyLevels.
        /// </summary>
        /// <param name="weaselInterceptor">The <c>IWeaselInterceptor</c> for the new <c>ProxyLevel</c>.</param>
        /// <param name="proxyScope">The scope on which the <c>IWeaselInterceptor</c> should intercept.</param>
        void PushProxyLevel(IWeaselInterceptor weaselInterceptor, IProxyScope proxyScope);
    }
}