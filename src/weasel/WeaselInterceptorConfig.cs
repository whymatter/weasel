using System;
using System.Reflection;

namespace weasel {
    internal class WeaselInterceptorConfig {
        public MethodInfo Target { get; }

        public int Order { get; }

        public Type Interceptor { get; }

        public InterceptorTypes InterceptorType { get; private set; }

        public WeaselInterceptorConfig(
            Type interceptor,
            InterceptorTypes interceptorType,
            int order,
            MethodInfo target = null) {
            if (interceptor == null) {
                throw new ArgumentNullException(nameof(interceptor));
            }

            Interceptor = interceptor;
            InterceptorType = interceptorType;
            Order = order;
            Target = target;
        }

        public bool IsCompatible(MethodInfo methodInfo) {
            return Target.Equals(methodInfo) || Target == null;
        }
    }
}