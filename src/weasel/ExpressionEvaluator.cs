using System;
using System.Linq.Expressions;

namespace weasel {
    /// <summary>
    ///     Evaluates an Expression
    /// </summary>
    internal class ExpressionEvaluator {
        public object Eval(Expression expression) {
            try {
                return Expression.Lambda<Func<object>>(expression).Compile()();
            }
            catch (Exception) {
                //todo
                throw;
            }
        }
    }
}