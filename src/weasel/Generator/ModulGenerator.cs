using System.Reflection.Emit;
using weasel.Core.Generator;

namespace weasel.Generator {
    /// <summary>
    ///     Generates a new DynamicAssembly.
    /// </summary>
    internal class ModuleGenerator : IModuleGenerator {
        /// <summary>
        ///     Generates a new <see cref="ModuleBuilder"/>.
        /// </summary>
        /// <param name="assemblyBuilder">The <see cref="AssemblyBuilder" /> for the new module.</param>
        /// <returns></returns>
        public ModuleBuilder GenerateModule(AssemblyBuilder assemblyBuilder) {
            var assemblyName = assemblyBuilder.GetName(false);
            return assemblyBuilder.DefineDynamicModule(assemblyName.Name, $"{assemblyName.Name}.dll");
        }
    }
}