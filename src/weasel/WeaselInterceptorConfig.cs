using System;
using System.Reflection;

namespace weasel {
    internal class WeaselInterceptorConfig {
        public MethodInfo Target { get; }

        public int Order { get; }

        public Type InterceptorType { get; }

        public WeaselInterceptorConfig(Type interceptorType, int order, MethodInfo target = null) {
            if (interceptorType == null) {
                throw new ArgumentNullException(nameof(interceptorType));
            }

            InterceptorType = interceptorType;
            Order = order;
            Target = target;
        }

        public bool IsCompatible(MethodInfo methodInfo) {
            return Target.Equals(methodInfo) || Target == null;
        }
    }
}