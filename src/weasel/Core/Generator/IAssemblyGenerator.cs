using System.Reflection.Emit;

namespace weasel.Core.Generator {
    internal interface IAssemblyGenerator {
        /// <summary>
        ///     Generates a new assembly.
        /// </summary>
        /// <returns></returns>
        AssemblyBuilder GenerateAssembly();
    }
}