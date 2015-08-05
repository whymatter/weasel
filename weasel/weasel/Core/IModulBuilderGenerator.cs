using System.Reflection.Emit;

namespace weasel.Core {
    /// <summary>
    ///     Generates a new DynamicAssembly.
    /// </summary>
    internal interface IModulBuilderGenerator {
        /// <summary>
        ///     Returns a ModulBuilder instance.
        /// </summary>
        /// <returns>ModuleBuilder</returns>
        ModuleBuilder ModuleBuilder { get; }
    }
}