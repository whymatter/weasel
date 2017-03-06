using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using weasel.Core.Generator;

namespace weasel.Generator {
    internal class MethodGenerator : IMethodGenerator {
        public void GenerateMethod(TypeBuilder typeBuilder, MethodInfo target, List<MethodGeneratorInfo> interceptors) {
            var overriddenMethod = typeBuilder
                .DefineMethod(
                    target.Name,
                    GetMethodAttributes(),
                    GetCallingConventions(),
                    target.ReturnType,
                    target.GetParameters().Select(p => p.ParameterType).ToArray());

            var afterCallInterceptors = FilterNSortInterceptors(interceptors, InterceptorTypes.AfterCall);
            var onExceptionInterceptors = FilterNSortInterceptors(interceptors, InterceptorTypes.OnException);
            var beforeReturnInterceptors = FilterNSortInterceptors(interceptors, InterceptorTypes.BeforeReturn);

            var ilGenerator = overriddenMethod.GetILGenerator();

            afterCallInterceptors.ForEach(i => {
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldfld, i.FieldBuilder);

                for (var j = 0; j < target.GetParameters().Length; j++) {
                    ilGenerator.Emit(OpCodes.Ldarg, j + 1);
                }

                ilGenerator.Emit(OpCodes.Callvirt, i.FieldBuilder.FieldType.GetMethod("Invoke"));
            });

            var returnValueField = ilGenerator.DeclareLocal(target.ReturnType);
            var exceptionLocal = ilGenerator.DeclareLocal(typeof(Exception));
            var continueLabel = ilGenerator.DefineLabel();

            if (!target.IsAbstract) {
                ilGenerator.BeginExceptionBlock();

                // load return value from base method
                GenerateBaseCall(ilGenerator, target);

                ilGenerator.Emit(OpCodes.Br, continueLabel);

                #region CATCH

                ilGenerator.BeginCatchBlock(typeof(Exception));

                ilGenerator.Emit(OpCodes.Stloc, exceptionLocal);

                onExceptionInterceptors.ForEach(i => {
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldfld, i.FieldBuilder);

                    for (var j = 0; j < target.GetParameters().Length; j++) {
                        ilGenerator.Emit(OpCodes.Ldarg, j + 1);
                    }

                    ilGenerator.Emit(OpCodes.Ldloc, exceptionLocal);

                    ilGenerator.Emit(OpCodes.Callvirt, i.FieldBuilder.FieldType.GetMethod("Invoke"));
                });

                // load and re-throw exception
                ilGenerator.Emit(OpCodes.Ldloc, exceptionLocal);
                ilGenerator.ThrowException(typeof(Exception));

                ilGenerator.EndExceptionBlock();

                #endregion

                // end catch
                ilGenerator.MarkLabel(continueLabel);

                // store
                ilGenerator.Emit(OpCodes.Stloc, returnValueField);
            }

            beforeReturnInterceptors.ForEach(i => {
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldfld, i.FieldBuilder);

                for (var j = 0; j < target.GetParameters().Length; j++) {
                    ilGenerator.Emit(OpCodes.Ldarg, j + 1);
                }

                ilGenerator.Emit(OpCodes.Ldloc, returnValueField);

                ilGenerator.Emit(OpCodes.Callvirt, i.FieldBuilder.FieldType.GetMethod("Invoke"));
            });

            ilGenerator.Emit(OpCodes.Ldloc, returnValueField);
            ilGenerator.Emit(OpCodes.Ret);
        }

        private static CallingConventions GetCallingConventions() => CallingConventions.HasThis;

        private static MethodAttributes GetMethodAttributes()
            => MethodAttributes.Public | MethodAttributes.ReuseSlot | MethodAttributes.Virtual | MethodAttributes.HideBySig;

        private List<MethodGeneratorInfo> FilterNSortInterceptors(List<MethodGeneratorInfo> interceptors, InterceptorTypes interceptorType) {
            return interceptors
                .Where(i => i.WeaselInterceptorConfig.InterceptorType == interceptorType)
                .OrderBy(i => i.WeaselInterceptorConfig.Order)
                .ToList();
        }

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

            public WeaselInterceptorConfig WeaselInterceptorConfig { get; }

            public FieldBuilder FieldBuilder { get; }
        }
    }
}