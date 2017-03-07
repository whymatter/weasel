using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using weasel.Internal.Core;
using weasel.Internal.Core.Generator;
using weasel.Internal.Generator;

namespace weasel.Internal {
    internal class ProxyBuilder<T> : IProxyBuilder<T> where T : class {
        private readonly ModuleBuilder _moduleBuilder;
        private readonly ITypeGenerator _typeGenerator;
        private readonly IFieldGenerator _fieldGenerator;
        private readonly AssemblyBuilder _proxyAssembly;

        private readonly List<ProxyBuilderConfig> _interceptorConfigs = new List<ProxyBuilderConfig>();
        private int _order = 0;

        public ProxyBuilder(ModuleBuilder moduleBuilder, ITypeGenerator typeGenerator, IFieldGenerator fieldGenerator, AssemblyBuilder proxyAssembly) {
            _moduleBuilder = moduleBuilder;
            _typeGenerator = typeGenerator;
            _fieldGenerator = fieldGenerator;
            _proxyAssembly = proxyAssembly;
        }

        public T Build(params object[] constructorPrams) {
            var proxyCandidate = typeof(T);
            var proxyClassBuilder = _typeGenerator.GenerateWrappingType(proxyCandidate, _moduleBuilder);

            var orderedInterceptorConfigs = _interceptorConfigs
                .OrderBy(config => config.TypeOfInterceptor).ThenBy(config => config.Order)
                .ToList();

            var privateFieldBuilders = orderedInterceptorConfigs
                .Select(config => _fieldGenerator.DefineField(proxyClassBuilder, config.InterceptorActionType))
                .ToList();

            var constructorInfos = proxyCandidate.GetConstructors().ToList();
            new ConstructorGenerator().CreateConstructor(proxyClassBuilder, constructorInfos, privateFieldBuilders);

            var methodGeneratorInfos = _interceptorConfigs
                .Select(config => new MethodGenerator.MethodGeneratorInfo(
                    config,
                    privateFieldBuilders
                        .Single(f => f.FieldType == config.InterceptorActionType)));

            var methodGenerator = new MethodGenerator();

            proxyCandidate.GetMethods().ToList().ForEach(m => {
                var concernedMethodGeneratorInfos = methodGeneratorInfos.Where(c => c.WeaselInterceptorConfig.IsCompatible(m)).ToList();

                if (!concernedMethodGeneratorInfos.Any()) return;

                if (!m.IsVirtual) {
                    throw new Exception($"Can't proxy non virtual method: {m.DeclaringType?.FullName}.{m}");
                }

                methodGenerator.GenerateMethod(proxyClassBuilder, m, concernedMethodGeneratorInfos);
            });

            var proxy = proxyClassBuilder.CreateType();
            new AssemblyPersistor().SaveAssembly(_proxyAssembly);
            return (T) Activator.CreateInstance(proxy, 6, "string", new Action<string, int>((s, i) => Debug.WriteLine($"Called with: {s}, {i}")));
        }

        /// <summary>
        /// Intercepts the method after call.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="expression"></param>
        /// <param name="interceptor"></param>
        /// <returns></returns>
        public IProxyBuilder<T> AfterCall<T1, T2, TR>(Expression<Func<T, Func<T1, T2, TR>>> expression, Action<T1, T2> interceptor) {
            _order++;

            var methodInfo = ExtractMethodInfo(expression);

            var newInterceptorConfig = new ProxyBuilderConfig(interceptor.GetType(), InterceptorTypes.AfterCall, _order, interceptor, methodInfo);
            _interceptorConfigs.Add(newInterceptorConfig);

            return this;
        }

        private static MethodInfo ExtractMethodInfo<T1, T2, TR>(Expression<Func<T, Func<T1, T2, TR>>> expression) {
            var unaryExpression = (UnaryExpression) expression.Body;
            var methodCallExpression = (MethodCallExpression) unaryExpression.Operand;
            var constantExpression = (ConstantExpression) methodCallExpression.Object;
            var methodInfo = (MethodInfo) constantExpression.Value;
            return methodInfo;
        }

        private class ProxyBuilderConfig : WeaselInterceptorConfig {
            public ProxyBuilderConfig(Type interceptorActionType, InterceptorTypes typeOfInterceptor, int order, object interceptor, MethodInfo targetMethod = null)
                : base(interceptorActionType, typeOfInterceptor, order, targetMethod) {
                Interceptor = interceptor;
            }

            public object Interceptor { get; set; }
        }
    }
}