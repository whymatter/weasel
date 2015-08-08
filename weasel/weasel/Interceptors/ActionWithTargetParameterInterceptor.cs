using System;
using weasel.Core;

namespace weasel.Interceptors {
    /// <summary>
    ///     An interceptor which calls an <c>Action</c> if the <c>IWeaselInterceptor</c> is executed.
    ///     The only parameter of the Action is the ProxyTargetType.
    /// </summary>
    internal class ActionWithTargetParameterInterceptor<TTarget> : IWeaselInterceptor {
        private Action<TTarget> _interceptorAction;

        /// <summary>
        ///     Creates an new ActionWithoutParameterInterceptor.
        /// </summary>
        /// <param name="interceptorAction">The <c>Action</c> to execute if the interceptor gets called.</param>
        public ActionWithTargetParameterInterceptor(Action<TTarget> interceptorAction) {
            if (interceptorAction == null) {
                throw new ArgumentNullException("interceptorAction");
            }

            _interceptorAction = interceptorAction;
        }
    }
}