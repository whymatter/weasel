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

        /// <summary>
        ///     Saves the assembly on disk.
        ///     Only avaliable if run in Debug Mode.
        /// </summary>
        /// <param name="fullPath">The full path, containing filename and extension.</param>
        void SaveAssembly(string fullPath);
    }
}