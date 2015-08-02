using System.Reflection.Emit;

namespace weasel.Core {
    internal interface IModulBuilderGenerator {
        /// <summary>
        ///     Returns a ModulBuilder instance.
        /// </summary>
        /// <returns></returns>
        ModuleBuilder GetModuleBuilder();
    }
}