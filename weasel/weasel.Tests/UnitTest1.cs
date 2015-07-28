using Microsoft.VisualStudio.TestTools.UnitTesting;
using weasel.Core;
using weasel.Core.Configuration;

namespace weasel.Tests {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestMethod1() {
            var newProxyBuilder = new ProxyProvider().CreateProxy<ClassToProxy>(new DefaultConfiguration());

            newProxyBuilder
                .ChainInterceptor(new Interceptor(), proxy => proxy.GetId())
                .ChainInterceptor(new Interceptor(), proxy => proxy.Id)
                .ChainInterceptor(new Interceptor(), proxy => proxy.ProcessData("key:=" + " 1"));
        }
    }

    public class Interceptor : IWeaselInterceptor {
        
    }

    public class ClassToProxy {
        public int Id { get; set; }

        public void ProcessData(string key) {}

        public int GetId() {
            return 1;
        }
    }

    public class ClassToProxyProxy : ClassToProxy {

    }
}