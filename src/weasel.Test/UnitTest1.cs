using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using weasel.Internal;
using weasel.Internal.Generator;

namespace weasel.Test {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestMethod1() {
            var configs = new List<WeaselInterceptorConfig> {
                new WeaselInterceptorConfig(typeof(Action<string, int>), InterceptorTypes.AfterCall, 1,
                    typeof(Bar).GetMethod(nameof(Bar.abc))),
                new WeaselInterceptorConfig(typeof(Action<string, int, Exception>), InterceptorTypes.OnException, 1,
                    typeof(Bar).GetMethod(nameof(Bar.abc))),
                new WeaselInterceptorConfig(typeof(Action<string, int, int>), InterceptorTypes.BeforeReturn, 1,
                    typeof(Bar).GetMethod(nameof(Bar.abc)))
            };

            var typeToWrap = typeof(Bar);

            var assemblyGenerator = new DynamicAssemblyGenerator();

            var proxyAssembly = assemblyGenerator.GenerateAssembly();

            var modulBuilderGenerator = new ModuleGenerator();

            var proxyModule = modulBuilderGenerator.GenerateModule(proxyAssembly);

            var timestampProvider = new TimestampProvider();
            var typeNameCreator = new TypeNameCreator(timestampProvider);

            var proxyTypeGenerator = new TypeGenerator(typeNameCreator);
            var wrappingType = proxyTypeGenerator.GenerateWrappingType(typeToWrap, proxyModule);

            var privateFieldGenerator = new PrivateFieldGenerator(typeNameCreator);
            var constructorBuilder = new ConstructorGenerator();

            var privateFieldBuilders = configs
                .Select(config => privateFieldGenerator.DefineField(wrappingType, config.InterceptorActionType))
                .ToList();

            constructorBuilder.CreateConstructor(wrappingType, typeToWrap.GetConstructors().ToList(), privateFieldBuilders);

            var methodGeneratorInfos = configs
                .Select(config => new MethodGenerator.MethodGeneratorInfo(
                    config,
                    privateFieldBuilders
                        .Single(f => f.FieldType == config.InterceptorActionType)));

            var methodGenerator = new MethodGenerator();

            typeToWrap.GetMethods().ToList().ForEach(m => {
                var concernedMethodGeneratorInfos = methodGeneratorInfos.Where(c => c.WeaselInterceptorConfig.IsCompatible(m)).ToList();

                if (!concernedMethodGeneratorInfos.Any()) return;

                if (!m.IsVirtual) {
                    throw new Exception($"Can't proxy non virtual method: {m.DeclaringType?.FullName}.{m}");
                }

                methodGenerator.GenerateMethod(wrappingType, m, concernedMethodGeneratorInfos);
            });


            var proxy = wrappingType.CreateType();

            var proxyInstance = (Bar) Activator.CreateInstance(proxy, 1, "test",
                new Action<string, int>((s, i) => Debug.WriteLine($"Called with: {s}, {i}")),
                new Action<string, int, Exception>((s, i, e) => Debug.WriteLine($"Exception: {e}")),
                new Action<string, int, int>((s, i, r) => Debug.WriteLine($"Return value: {r}")));

            var persitor = new AssemblyPersistor();
            persitor.SaveAssembly(proxyAssembly);


            var expected = proxyInstance.abc("test", 01);

            try {
                expected = proxyInstance.abc("test", 77);
            }
            catch (Exception e) {}


            var fooInstance = new Foo(1, "");
            Assert.AreEqual(1, expected);

            SetUp<int>
                .CreateSetUp<Bar>()
                .Setup(b => new Func<string, int, int>(b.abc), (a, b, c) => Debug.WriteLine($"{a}"));
        }

        public void Test(params object[] p)
        {
            Debug.WriteLine(p.Length);
        }

        [TestMethod]
        public void ProxyBuilder_Success() {
            var proxyCollection = new ProxyCollection();

            var proxy = proxyCollection
                .CreateProxy<Bar>()
                .AfterCall(b => new System.Func<string, int, int>(b.abc), (s, i) => Debug.WriteLine($"Called with: {s}, {i}"))
                
                .Build(66, "Hello proxy");
        }
    }

    public delegate TResult Func<in T1, in T2, out TResult>(T1 arg1, T2 arg2);

    public class Bar {
        public Bar(int i, string b) {}

        public virtual int abc(string bvg, int m) {
            if (m == 77) {
                throw new NotImplementedException("this is not wanted!");
            }

            return m;
        }
    }

    internal class Foo : Bar {
        private readonly Action<string, int> _a1 = A1;

        private static void A1(string s, int i) {}

        public Foo(int i, string b) : base(i, b) {}

        public override int abc(string bvg, int m) {
            //_a1(bvg, m);
            var a = base.abc(bvg, m);
            return 7;
        }
    }

    public class SetUp<TProxy> {
        public static void DemoSetup() {}

        public static SetUp<TP> CreateSetUp<TP>() {
            return new SetUp<TP>();
        }

        public MemberInfo Setup<T1, T2, TR>(Expression<Func<TProxy, Func<T1, T2, TR>>> expression, Action<T1, T2, TR> interceptor) {
            var unaryExpression = (UnaryExpression) expression.Body;
            var methodCallExpression = (MethodCallExpression) unaryExpression.Operand;
            var methodInfoExpression = (ConstantExpression) methodCallExpression.Object;
            var methodInfo = (MemberInfo) methodInfoExpression.Value;
            return methodInfo;
        }

        //public void Setup<TArg1, TArg2, TResult>(Expression<Func<TProxy, Func<TArg1, TArg2, TResult>>> target, Func<TArg1, TArg2> interceptor)
        //{

        //}

        public void Setup<TArg1, TArg2, TResult>(Func<TArg1, TArg2, TResult> target, Func<TArg1, TArg2> interceptor) {}
    }

    public class ClassToProxy {
        public int Id { get; set; }
        public void ProcessData(string key) {}

        public int GetId() {
            return 1;
        }
    }

    public class Proxy : ClassToProxy {
        private readonly ClassToProxy _original;

        public Proxy(ClassToProxy original) {
            _original = original;
        }

        public override int GetHashCode() {
            return _original?.GetHashCode() ?? 0;
        }

        public static bool operator ==(Proxy proxy, ClassToProxy classTo) {
            return proxy._original == classTo;
        }

        public static bool operator !=(Proxy proxy, ClassToProxy classTo) {
            return !(proxy == classTo);
        }

        public override bool Equals(object obj) {
            return obj == _original;
        }
    }
}