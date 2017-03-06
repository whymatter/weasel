using System.Reflection.Emit;

namespace weasel.Core.Generator {
    /// <summary>
    ///     Generates a new <see cref="ModuleBuilder"/>.
    /// </summary>
    internal interface IModuleGenerator {
        /// <summary>
        ///     Generates a new <see cref="ModuleBuilder"/>.
        /// </summary>
        /// <param name="assemblyBuilder">The <see cref="AssemblyBuilder" /> for the new module.</param>
        /// <returns></returns>
        ModuleBuilder GenerateModule(AssemblyBuilder assemblyBuilder);
    }
}