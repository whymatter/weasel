using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace weasel.Internal.Core.Generator {
    /// <summary>
    /// Strategie to generate a constructor for a proxy type.
    /// </summary>
    internal interface IConstructorGenerator {
        /// <summary>
        ///     Creates all constructors needed.
        /// </summary>
        /// <param name="typeBuilder">The <see cref="TypeBuilder"/> for the proxy class.</param>
        /// <param name="constructorInfos">All existing <see cref="ConstructorInfo"/>'s on the class to proxy.</param>
        /// <param name="interceptors">The fields of all interceptors.</param>
        void CreateConstructor(TypeBuilder typeBuilder, List<ConstructorInfo> constructorInfos,
            List<FieldBuilder> interceptors);
    }
}