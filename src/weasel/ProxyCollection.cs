using System.Reflection.Emit;
using weasel.Internal;
using weasel.Internal.Core.Generator;
using weasel.Internal.Generator;

namespace weasel {
    public class ProxyCollection {
        private AssemblyBuilder _proxyAssembly;
        private ModuleBuilder _proxyModule;
        private TypeGenerator _proxyTypeGenerator;
        private readonly IFieldGenerator _fieldGenerator;

        private readonly TypeNameCreator _typeNameCreator;

        public ProxyCollection() {
            var timestampProvider = new TimestampProvider();
            _typeNameCreator = new TypeNameCreator(timestampProvider);
            _fieldGenerator = new PrivateFieldGenerator(_typeNameCreator);

            SetupAssembly();
        }

        private void SetupAssembly() {
            _proxyAssembly = new DynamicAssemblyGenerator().GenerateAssembly();
            _proxyModule = new ModuleGenerator().GenerateModule(_proxyAssembly);

            _proxyTypeGenerator = new TypeGenerator(_typeNameCreator);
        }

        /// <summary>
        /// Creates a <see cref="IProxyBuilder{T}"/> for a new proxy class of type <typeparam name="T"></typeparam>.
        /// </summary>
        /// <typeparam name="T">Type of the class to proxy.</typeparam>
        /// <returns></returns>
        public IProxyBuilder<T> CreateProxy<T>() where T : class {
            return new ProxyBuilder<T>(_proxyModule, _proxyTypeGenerator, _fieldGenerator, _proxyAssembly);
        }
    }
}