using System.Reflection;
using System.Reflection.Emit;
using weasel.Core;

namespace weasel.Assembler {
    /// <summary>
    /// Assembles the constructors for the proxy.
    /// </summary>
    internal class ConstructorAssembler {
        private readonly IModulBuilderGenerator _modulBuilderGenerator;

        public ConstructorAssembler(IModulBuilderGenerator modulBuilderGenerator) {
            _modulBuilderGenerator = modulBuilderGenerator;
        }

        public void CreateConstructor(TypeBuilder proxyClassBuilder) {
            
        }
    }
}