using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using weasel.Generator;

namespace weasel.Core.Generator {
    internal interface IMethodGenerator {
        void GenerateMethod(TypeBuilder typeBuilder, MethodInfo target, List<MethodGenerator.MethodGeneratorInfo> interceptors);
    }
}