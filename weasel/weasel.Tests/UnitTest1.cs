using System;
using System.Diagnostics;
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
                .ChainInterceptor(() => EventLog.WriteEntry("", ""), proxy => proxy.GetId())
                .ChainInterceptor(proxy => EventLog.WriteEntry("", ""), proxy => proxy.GetId())
                .ChainInterceptor(new Interceptor(), proxy => proxy.ProcessData("k"))
                .ChainInterceptor(proxy => EventLog.WriteEntry("", ""), proxy => proxy.ProcessData("k"))
                .ChainInterceptor(proxy => EventLog.WriteEntry("", ""), proxy => proxy.GetId())
                //proxy => proxy.ProcessData("k")
                .ChainInterceptor(() => EventLog.WriteEntry("", ""), proxy => proxy.ProcessData("k"));

            new Bar<Foo>().Run(foo => foo.RunMethod());
            new Bar<Foo>().Run(foo => foo.RunFunction());
        }
    }

    internal class Bar<TFoo> {
        public void Run<TReturn>(Func<TFoo, TReturn> scope) {}

        public void Run(Action<TFoo> scope) {}

        public void Run(Action scope) {}
    }

    internal class Foo {
        public void RunMethod() {}

        public int RunFunction() {}
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
            return (_original != null ? _original.GetHashCode() : 0);
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