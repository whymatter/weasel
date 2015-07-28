using System;

namespace weasel.Core.Exceptions {
    /// <summary>
    ///     The Expression with the passed name is an invalid MethodCallExpression.
    /// </summary>
    internal class InvalidScopeExpressionException : ArgumentException {
        /// <summary>
        ///     Creats an InvalidScopeExpressionException
        /// </summary>
        /// <param name="parameterName">The name of the parameter with the invalid expression.</param>
        public InvalidScopeExpressionException(string parameterName)
            : base("The " + parameterName + " contains a invalid ScopeExpression!") {}
    }
}