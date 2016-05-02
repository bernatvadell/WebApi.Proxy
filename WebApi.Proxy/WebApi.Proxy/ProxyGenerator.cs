using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Proxy.Interceptors;

namespace WebApi.Proxy
{
    public class ProxyGenerator<T> where T : class
    {
        private ProxyGenerator _generator;
        private WebApiConfiguration _configuration;

        public ProxyGenerator(WebApiConfiguration configuration)
        {
            if (!typeof(T).IsInterface)
                throw new NotSupportedException(string.Format("type {0} is not an interface", typeof(T).FullName));

            if (configuration == null)
                throw new ArgumentNullException("configuration");

            _generator = new ProxyGenerator();
            _configuration = configuration;
        }

        public T Build()
        {
            return _generator
                .CreateInterfaceProxyWithoutTarget<T>(new WebApiInterceptor(_configuration));
        }
    }
}
