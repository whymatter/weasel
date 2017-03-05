using System;
using weasel.Core;

namespace weasel.Interceptors {
    /// <summary>
    ///     An interceptor which calls an <c>Action</c> if the <c>IWeaselInterceptor</c> is executed.
    /// </summary>
    internal class ActionWithoutParameterInterceptor : IWeaselInterceptor {
        private Action _interceptorAction;

        /// <summary>
        ///     Creates an new ActionWithoutParameterInterceptor.
        /// </summary>
        /// <param name="interceptorAction">The <c>Action</c> to execute if the interceptor gets called.</param>
        public ActionWithoutParameterInterceptor(Action interceptorAction) {
            if (interceptorAction == null) {
                throw new ArgumentNullException("interceptorAction");
            }

            _interceptorAction = interceptorAction;
        }
    }
}