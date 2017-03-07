using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using weasel.Internal.Generator;

namespace weasel.Internal.Core.Generator {
    internal interface IMethodGenerator {
        void GenerateMethod(TypeBuilder typeBuilder, MethodInfo target, List<MethodGenerator.MethodGeneratorInfo> interceptors);
    }
}