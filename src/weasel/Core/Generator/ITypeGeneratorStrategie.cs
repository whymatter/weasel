using System;
using System.Reflection.Emit;

namespace weasel.Core.Generator {
    internal interface ITypeGeneratorStrategie {
        /// <summary>
        ///     Assembles a new type.
        /// </summary>
        /// <param name="typeToWrap">The interface as type which should be wrapped.</param>
        /// <param name="moduleBuilder">The <see cref="ModuleBuilder"/> to use.</param>
        /// <returns></returns>
        TypeBuilder CreateType(Type typeToWrap, ModuleBuilder moduleBuilder);
    }
}