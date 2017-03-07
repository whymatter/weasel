using System.Reflection.Emit;

namespace weasel.Internal.Core {
    internal interface IAssemblyPersistor {
        /// <summary>
        ///     Saves the assembly on disk.
        ///     Only avaliable if run in Debug Mode.
        /// </summary>
        /// <param name="builder"></param>
        void SaveAssembly(AssemblyBuilder builder);
    }
}