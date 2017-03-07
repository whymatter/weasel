using System;
using System.Reflection.Emit;

namespace weasel.Internal.Core {
    internal interface ITypeGenerator {
        /// <summary>
        ///     Creates a new type which is wrapping the <param name="typeToWrap"></param>.
        /// </summary>
        /// <param name="typeToWrap">The Type for which the wrapping Type should be created.</param>
        /// <param name="moduleBuilder">The <see cref="ModuleBuilder"/> to build the wrapping type.</param>
        /// <returns></returns>
        TypeBuilder GenerateWrappingType(Type typeToWrap, ModuleBuilder moduleBuilder);
    }
}