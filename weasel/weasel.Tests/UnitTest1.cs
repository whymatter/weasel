using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace weasel.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var typePool = new TypePool();
            typePool.InitializeDynamicAssembly();

            Assert.AreEqual(42, typePool.CreateProxyOfType(new ClassToProxy()).MethodShouldBeLogged());

            Assert.AreEqual(3, typePool.CreateProxyOfType(new ClassToProxyProxy()).MethodShouldBeLogged());

            Assert.AreEqual(3, new ClassToProxyProxy().NonVirtualMethod());
        }
    }

    public class ClassToProxy
    {
        public int MyIntProperty { get; set; }

        public virtual int MethodShouldBeLogged()
        {
            return 3;
        }

        public int NonVirtualMethod()
        {
            return 3;
        }
    }

    public class ClassToProxyProxy : ClassToProxy
    {
        public new int MyIntProperty { get; set; }

        public new int NonVirtualMethod()
        {
            return base.NonVirtualMethod();
        }
    }
}