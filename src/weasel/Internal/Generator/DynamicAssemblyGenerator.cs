using System;
using System.Reflection;
using System.Reflection.Emit;
using weasel.Internal.Core.Generator;

namespace weasel.Internal.Generator {
    internal class DynamicAssemblyGenerator : IAssemblyGenerator {
#if DEBUG
        private const AssemblyBuilderAccess AccessLevel = AssemblyBuilderAccess.RunAndSave;
#else
        private const AssemblyBuilderAccess AccessLevel = AssemblyBuilderAccess.Run;
#endif

        /// <summary>
        ///     Generates a new dynamic assembly.
        /// </summary>
        /// <returns></returns>
        public AssemblyBuilder GenerateAssembly() {
            var assemblyName = GetNewAssemblyName();
            return AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AccessLevel);
        }

        /// <summary>
        ///     Creates an new AssemblyName.
        /// </summary>
        /// <returns></returns>
        private AssemblyName GetNewAssemblyName() {
            return new AssemblyName($"weasel.{Math.Abs(DateTime.Now.ToBinary())}_DYNAMIC");
        }
    }
}