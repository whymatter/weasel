using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using weasel.Core;

namespace weasel {
    internal class ProxyChain : IProxyChain {
        private readonly List<ProxyLevel> _proxyLevels = new List<ProxyLevel>();

        /// <summary>
        ///     The single <c>ProxyLevel</c> steps.
        /// </summary>
        public ReadOnlyCollection<ProxyLevel> ProxyLevels {
            get { return _proxyLevels.AsReadOnly(); }
        }

        /// <summary>
        ///     Pushes a new <c>ProxyLevel</c> into the ProxyLevels.
        /// </summary>
        /// <param name="newProxyLevel">The new ProxyLevel instance.</param>
        public void PushProxyLevel(ProxyLevel newProxyLevel) {
            if (newProxyLevel == null) {
                throw new ArgumentNullException("newProxyLevel");
            }

            _proxyLevels.Add(newProxyLevel);
        }

        /// <summary>
        ///     Creates a new <c>ProxyLevel</c> and pushes it into the ProxyLevels.
        /// </summary>
        /// <param name="weaselInterceptor">The <c>IWeaselInterceptor</c> for the new <c>ProxyLevel</c>.</param>
        /// <param name="proxyScope">The scope on which the <c>IWeaselInterceptor</c> should intercept.</param>
        public void PushProxyLevel(IWeaselInterceptor weaselInterceptor, IProxyScope proxyScope) {
            _proxyLevels.Add(new ProxyLevel(weaselInterceptor, proxyScope));
        }
    }
}