using System;
using System.Reflection;
using System.Reflection.Emit;
using weasel.Core;

namespace weasel {
    /// <summary>
    ///     Generates a new DynamicAssembly
    /// </summary>
    internal class AssemblyGenerator : IAssemblyGenerator {
#if DEBUG
        private const AssemblyBuilderAccess AccessLevel = AssemblyBuilderAccess.RunAndSave;
#else
        private const AssemblyBuilderAccess AccessLevel = AssemblyBuilderAccess.Run;
#endif

        /// <summary>
        ///     Generates a new AssemblyBuilder
        /// </summary>
        /// <returns></returns>
        public AssemblyBuilder CreateAssembly() {
            var assemblyName = GetNewAssemblyName();
            return AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AccessLevel);
        }

        /// <summary>
        ///     Generates a new ModuleBuilder
        /// </summary>
        /// <param name="builder">The AssemblyBuilder in which the module should be created</param>
        /// <returns></returns>
        public ModuleBuilder GetModuleBuilder(AssemblyBuilder builder) {
            var assemblyName = builder.GetName();
            return builder.DefineDynamicModule(assemblyName.Name, string.Format("{0}.dll", assemblyName.Name));
        }

        /// <summary>
        ///     Creates an new AssemblyName
        /// </summary>
        /// <returns></returns>
        private AssemblyName GetNewAssemblyName() {
            return new AssemblyName(string.Format("weasel.{0}_DYNAMIC", DateTime.Now.ToBinary()));
        }
    }
}