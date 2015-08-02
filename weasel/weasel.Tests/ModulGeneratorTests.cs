using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace weasel.Tests {
    [TestClass]
    public class ModulGeneratorTests {
        [TestMethod]
        public void CreatedSingleton_NotNull() {
            ModulBuilderGenerator.GetAssemblyGenerator().Should().NotBeNull();
        }

        [TestMethod]
        public void CreatedSingleton_TwoTimesSameInstance() {
            var modulBuilderGenerator = ModulBuilderGenerator.GetAssemblyGenerator();
            var modulBuilderGenerator2 = ModulBuilderGenerator.GetAssemblyGenerator();
            modulBuilderGenerator.Should().Be(modulBuilderGenerator2);
        }

        [TestMethod]
        public void GetModuleBuilder_NotNull() {
            ModulBuilderGenerator.GetAssemblyGenerator().GetModuleBuilder().Should().NotBeNull();
        }

        [TestMethod]
        public void GetModuleBuilder_TwoTimesSameInstance() {
            var modulBuilder = ModulBuilderGenerator.GetAssemblyGenerator().GetModuleBuilder();
            var modulBuilder2 = ModulBuilderGenerator.GetAssemblyGenerator().GetModuleBuilder();
            modulBuilder.Should().Be(modulBuilder2);
        }
    }
}