using Microsoft.VisualStudio.TestTools.UnitTesting;
using weasel.Core;

namespace weasel.Tests {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestMethod1() {
            //var newProxyBuilder = new ProxyProvider().CreateProxy<ClassToProxy>(new DefaultConfiguration());

            //newProxyBuilder
            //    .ChainInterceptor(new Interceptor(), proxy => proxy.GetId())
            //    .ChainInterceptor(new Interceptor(), proxy => proxy.Id)
            //    .ChainInterceptor(new Interceptor(), proxy => proxy.ProcessData("key:=" + " 1"));

            var original = new ClassToProxy();
            ClassToProxy proxy = new Proxy(original);

            Assert.IsTrue(proxy == original);
            //Assert.IsTrue(original.Equals(proxy));
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

        public override int GetHashCode() {
            return (_original != null ? _original.GetHashCode() : 0);
        }

        private readonly ClassToProxy _original;

        public Proxy(ClassToProxy original) {
            _original = original;
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