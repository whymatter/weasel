using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace weasel.Scopes {
    /// <summary>
    ///     Encapsulates the IWeaselInterceptor on a Method
    /// </summary>
    internal class MethodScope : IProxyScope {
        /// <summary>
        ///     List of all method parameters
        /// </summary>
        private readonly IEnumerable<Expression> _methodParameters;

        /// <summary>
        ///     This List gets initialized by calling the ParameterValues property
        /// </summary>
        private List<object> _compiledParameters;

        /// <summary>
        ///     Creates the MethodScope
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="methodParameters"></param>
        public MethodScope(MethodInfo methodInfo, IEnumerable<Expression> methodParameters) {
            Method = methodInfo;
            _methodParameters = methodParameters;
        }

        /// <summary>
        ///     The method on which the interceptor is scoped
        /// </summary>
        public MethodInfo Method { get; private set; }

        /// <summary>
        ///     Defines if the interceptor is scoped on the parameters
        /// </summary>
        public bool MatchByParameter { get; set; }

        /// <summary>
        ///     The Parameter for the ParameterScope
        /// </summary>
        public List<object> ParameterValues {
            get {
                return _compiledParameters ??
                       (_compiledParameters = _methodParameters.Select(p => new ExpressionEvaluator().Eval(p)).ToList());
            }
        }
    }
}