using Microsoft.VisualStudio.TestTools.UnitTesting;
using weasel.Core;
using weasel.Generator;

namespace weasel.Test {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestMethod1() {
            var assemblyGenerator = new DynamicAssemblyGenerator();
            var modulBuilderGenerator = new ModuleGenerator();
            var timestampProvider = new TimestampProvider();
            var typeNameCreator = new TypeNameCreator(timestampProvider);

            new ProxyTypeGenerator(modulBuilderGenerator, typeNameCreator)
                .GenerateWrappingType<Bar>();
        }
    }

    public class Bar {

        public Bar(int i, string b)
        {

        }

    }

    internal class Foo : Bar {
        private int _i;
        public Foo(int i, string b, int a) : base(i, b) {
            _i = a;
        }

        private int z() {
            return _i;
        }
    }

    public class Interceptor : IWeaselInterceptor {}

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