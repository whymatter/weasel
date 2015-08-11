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

            new TypeGenerator(new ModulBuilderGenerator(), new TypeNameCreator(new TimestampProvider())).GenerateWrappingType(typeof (Bar),
                null);



        }
    }

    public class Bar {

        public Bar(int i, string b) {
            
        }

    }

    internal class Foo {
        public Foo() {
            
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