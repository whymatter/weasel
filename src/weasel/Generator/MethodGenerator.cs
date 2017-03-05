using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace weasel.Generator {
    public delegate void BeforeInterceptor(params object[] parameters);

    internal class MethodGenerator {
        public void GenerateMethod(TypeBuilder typeBuilder, MethodInfo target, List<MethodGeneratorInfo> interceptors) {
            var methodInfoBody = typeBuilder
                .DefineMethod(
                    target.Name,
                    GetMethodAttributes(),
                    GetCallingConventions(),
                    target.ReturnType,
                    target.GetParameters().Select(p => p.ParameterType).ToArray());

            var methodIlGenerator = methodInfoBody.GetILGenerator();

            interceptors.ForEach(i => {
                methodIlGenerator.Emit(OpCodes.Ldarg_0);
                methodIlGenerator.Emit(OpCodes.Ldfld, i.FieldBuilder);

                for (var j = 0; j < target.GetParameters().Length; j++) {
                    methodIlGenerator.Emit(OpCodes.Ldarg, j + 1);
                }

                methodIlGenerator.Emit(OpCodes.Callvirt, i.FieldBuilder.FieldType.GetMethod("Invoke"));
            });
            

            GenerateBaseCall(methodIlGenerator, target);
            

            methodIlGenerator.Emit(OpCodes.Ret);
        }

        private static CallingConventions GetCallingConventions() => CallingConventions.HasThis;

        private static MethodAttributes GetMethodAttributes()
            => MethodAttributes.Public | MethodAttributes.ReuseSlot | MethodAttributes.Virtual | MethodAttributes.HideBySig;

        private void GenerateBaseCall(ILGenerator generator, MethodInfo baseMethod) {
            var parameters = baseMethod.GetParameters().Select(p => p.ParameterType).ToList();

            generator.Emit(OpCodes.Ldarg_0);

            for (var i = 0; i < parameters.Count; i++) {
                generator.Emit(OpCodes.Ldarg, i + 1);
            }

            generator.Emit(OpCodes.Call, baseMethod);
        }

        public class MethodGeneratorInfo {
            public MethodGeneratorInfo(WeaselInterceptorConfig weaselInterceptorConfig, FieldBuilder fieldBuilder) {
                WeaselInterceptorConfig = weaselInterceptorConfig;
                FieldBuilder = fieldBuilder;
            }

            public WeaselInterceptorConfig WeaselInterceptorConfig { get; set; }

            public FieldBuilder FieldBuilder { get; set; }
        }
    }
}