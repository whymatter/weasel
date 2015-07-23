using System.Reflection.Emit;

namespace weasel.Core
{
    internal interface IAssemblyGenerator
    {
        /// <summary>
        ///     Generates a new AssemblyBuilder
        /// </summary>
        /// <returns></returns>
        AssemblyBuilder CreateAssembly();

        /// <summary>
        ///     Generates a new ModuleBuilder
        /// </summary>
        /// <param name="builder">The AssemblyBuilder in which the module should be created</param>
        /// <returns></returns>
        ModuleBuilder GetModuleBuilder(AssemblyBuilder builder);
    }
}