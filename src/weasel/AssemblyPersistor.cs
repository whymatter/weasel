﻿using System.Reflection.Emit;
using weasel.Core;

namespace weasel {
    internal class AssemblyPersistor : IAssemblyPersistor {
        /// <summary>
        ///     Saves the assembly on disk.
        ///     Only avaliable if run in Debug Mode.
        /// </summary>
        /// <param name="builder"></param>
        public void SaveAssembly(AssemblyBuilder builder) {
#if DEBUG
            builder.Save($"{builder.FullName}.dll");
#endif
        }
    }
}