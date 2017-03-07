using System;
using System.Reflection;

namespace weasel.Internal {
    internal class WeaselInterceptorConfig {
        public MethodInfo TargetMethod { get; }

        public int Order { get; }

        public Type InterceptorActionType { get; }

        public InterceptorTypes TypeOfInterceptor { get; private set; }

        public WeaselInterceptorConfig(
            Type interceptorActionType,
            InterceptorTypes typeOfInterceptor,
            int order,
            MethodInfo targetMethod = null) {
            if (interceptorActionType == null) {
                throw new ArgumentNullException(nameof(interceptorActionType));
            }

            InterceptorActionType = interceptorActionType;
            TypeOfInterceptor = typeOfInterceptor;
            Order = order;
            TargetMethod = targetMethod;
        }

        public bool IsCompatible(MethodInfo methodInfo) {
            return TargetMethod.Equals(methodInfo) || TargetMethod == null;
        }
    }
}