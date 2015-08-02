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

        /// <summary>
        /// This is the singleton instance of the IModulBuilderGenerator
        /// </summary>
        private static readonly IModulBuilderGenerator ModulBuilderGeneratorInstance = new ModulBuilderGenerator();

        private readonly ModuleBuilder _moduleBuilder;

        /// <summary>
        /// Get the singleton.
        /// </summary>
        /// <returns></returns>
        public static IModulBuilderGenerator GetAssemblyGenerator() {
            return ModulBuilderGeneratorInstance;
        }

        /// <summary>
        /// Singleton -> No public constructor.
        /// </summary>
        private ModulBuilderGenerator() {
            // Create the new dynamic assembly
            var assemblyName = GetNewAssemblyName();
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AccessLevel);
            _moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, string.Format("{0}.dll", assemblyName.Name));
        }

        /// <summary>
        ///     Returns a ModulBuilder instance.
        /// </summary>
        /// <returns></returns>
        public ModuleBuilder GetModuleBuilder() {
            return _moduleBuilder;
        }

        /// <summary>
        ///     Creates an new AssemblyName.
        /// </summary>
        /// <returns></returns>
        private AssemblyName GetNewAssemblyName() {
            return new AssemblyName(string.Format("weasel.{0}_DYNAMIC", DateTime.Now.ToBinary()));
        }
    }
}