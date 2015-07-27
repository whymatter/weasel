using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace weasel.Tests {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestMethod1() {
            //new ProxyProvider().CreateProxy<ClassToProxy>(new DefaultConfiguration())
            //    .ChainInterceptor(null, proxy => proxy.MethodShouldBeLogged(1));

            Methode(t => t.VoidMethod());
            Methode(t => t.IntMethod(1));
            Methode(t => t.IntPropertys);

            Expression(t => t.VoidMethod());
            Expression(t => t.IntMethod(1));
            Expression(t => t.IntPropertys);
        }

        public void Methode(Action<ClassToProxy> t) {}
        public void Methode<TReturn>(Func<ClassToProxy, TReturn> t) {}

        public void Expression<TReturn>(Expression<Func<ClassToProxy, TReturn>> t)
        {
            
        }

        public void Expression(Expression<Action<ClassToProxy>> t)
        {
            
        }
    }

    public class ClassToProxy {
        public int IntPropertys { get; set; }
        public void VoidMethod() {}

        public int IntMethod() {
            return 1;
        }

        public int IntMethod(int i) {
            return 1;
        }
    }

    public class ClassToProxyProxy : ClassToProxy {
        public int NonVirtualMethod() {
            //return base.NonVirtualMethod();
            return 1;
        }
    }
}