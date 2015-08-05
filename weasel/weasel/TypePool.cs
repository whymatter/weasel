using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace weasel {
    public class TypePool {
        private ModuleBuilder _builder;
        private AssemblyBuilder _dynamicAssemblyBuilder;
        private AssemblyName _dynamicAssemblyName;

        public void InitializeDynamicAssembly() {
            var dynamicAssemblyName = new AssemblyName("weasel.DynamicProxy");
            var dynamicAssemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(dynamicAssemblyName,
                AssemblyBuilderAccess.RunAndSave);
            var mb = dynamicAssemblyBuilder.DefineDynamicModule(dynamicAssemblyName.Name,
                dynamicAssemblyName.Name + ".dll");

            _builder = mb;
            _dynamicAssemblyName = dynamicAssemblyName;
            _dynamicAssemblyBuilder = dynamicAssemblyBuilder;
        }

        public TTarget CreateProxyOfType<TTarget>(TTarget target) where TTarget : class {
            if (target == null) {
                throw new ArgumentNullException("target");
            }

            var targetType = typeof (TTarget);

            var dynamicTypeBuilder = GetNewDynamicClassType(targetType);
            var virtualMethods = GetVirtualMethods(targetType);

            // Methoden überschreiben
            foreach (var virtualMethod in virtualMethods) {
                OverrideVirtualMethod(dynamicTypeBuilder, virtualMethod);
            }

            return new ProxyActivator().CreateInstance<TTarget>(dynamicTypeBuilder.CreateType());
        }

        private void OverrideVirtualMethod(TypeBuilder typeBuilder, MethodInfo methodToOverride) {
            var returnType = GetReturnTypeOfMethod(methodToOverride);
            var methodParameters = GetMethodParameter(methodToOverride);

            var overridingMethod = typeBuilder.DefineMethod(GetOverriddenMethodName(methodToOverride),
                GetMethodAttributes(),
                CallingConventions.HasThis,
                returnType,
                methodParameters.ToArray());

            var ilEmitter = overridingMethod.GetILGenerator();

            ilEmitter.Emit(OpCodes.Ldc_I4, 42);
            ilEmitter.Emit(OpCodes.Ret);
        }

        private Type GetReturnTypeOfMethod(MethodInfo methodInfo) {
            return methodInfo.ReturnType;
        }

        private string GetOverriddenMethodName(MethodInfo overriddenMethod) {
            return string.Format("{0}", overriddenMethod.Name);
        }

        private List<Type> GetMethodParameter(MethodInfo methodInfo) {
            return methodInfo.GetParameters().Select(p => p.ParameterType).ToList();
        }

        private List<MethodInfo> GetVirtualMethods(Type targetType) {
            return
                targetType.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance)
                    .Where(method => method.IsVirtual)
                    .ToList();
        }

        private TypeBuilder GetNewDynamicClassType(Type targetType) {
            return _builder.DefineType(GetDynamicProxyName(targetType), GetTypeAttributesForDynamicClass(), targetType);
        }

        private string GetDynamicProxyName(Type targetType) {
            return string.Format("weasel.DynamicProxy.{0}_DYNAMIC", targetType.Name);
        }

        private MethodAttributes GetMethodAttributes() {
            return MethodAttributes.Public | MethodAttributes.ReuseSlot | MethodAttributes.Virtual |
                   MethodAttributes.HideBySig;
        }

        private TypeAttributes GetTypeAttributesForDynamicClass() {
            return TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed;
        }
    }
}