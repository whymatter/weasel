using System;
using System.Reflection.Emit;

namespace weasel.Core.Assembler {
    internal interface ITypeAssembler {
        /// <summary>
        ///     Assembles a new wrapping type for the typeToWrap.
        /// </summary>
        /// <param name="typeToWrap"></param>
        /// <returns></returns>
        TypeBuilder CreateType(Type typeToWrap);
    }
}