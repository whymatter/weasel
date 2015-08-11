using System;
using System.Reflection;
using System.Reflection.Emit;
using weasel.Core;

namespace weasel {
    /// <summary>
    ///     Generates a new DynamicAssembly.
    /// </summary>
    internal class ModulBuilderGenerator : IModulBuilderGenerator {
#if DEBUG
        private const AssemblyBuilderAccess AccessLevel = AssemblyBuilderAccess.RunAndSave;
#else
        private const AssemblyBuilderAccess AccessLevel = AssemblyBuilderAccess.Run;
#endif

        private readonly ModuleBuilder _moduleBuilder;
        private readonly AssemblyBuilder _assemblyBuilder;

        /// <summary>
        ///     Creates the ModulBuilderGenerator.
        /// </summary>
        public ModulBuilderGenerator() {
            // Create the new dynamic assembly
            var assemblyName = GetNewAssemblyName();
            _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AccessLevel);
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(assemblyName.Name, string.Format("{0}.dll", assemblyName.Name));
        }

        /// <summary>
        ///     Returns a ModulBuilder instance.
        /// </summary>
        /// <returns>ModuleBuilder</returns>
        public ModuleBuilder ModuleBuilder {
            get { return _moduleBuilder; }
        }

        /// <summary>
        ///     Saves the assembly on disk.
        ///     Only avaliable if run in Debug Mode.
        /// </summary>
        /// <param name="fullPath">The full path, containing filename and extension.</param>
        public void SaveAssembly(string fullPath) {
#if DEBUG
            _assemblyBuilder.Save(fullPath);
#endif
        }

        /// <summary>
        ///     Creates an new AssemblyName.
        /// </summary>
        /// <returns></returns>
        private AssemblyName GetNewAssemblyName() {
            return new AssemblyName(string.Format("weasel.{0}_DYNAMIC", Math.Abs(DateTime.Now.ToBinary())));
        }
    }
}