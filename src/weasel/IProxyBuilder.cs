using System;
using System.Linq.Expressions;

namespace weasel {
    public interface IProxyBuilder<T> where T : class {
        T Build(params object[] constructorPrams);

        /// <summary>
        /// Intercepts the method after call.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="expression"></param>
        /// <param name="interceptor"></param>
        /// <returns></returns>
        IProxyBuilder<T> AfterCall<T1, T2, TR>(Expression<Func<T, Func<T1, T2, TR>>> expression, Action<T1, T2> interceptor);
    }
}