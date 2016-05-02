using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebApi.Proxy.Attributes;

namespace WebApi.Proxy.Interceptors
{
    public class WebApiInterceptor : IInterceptor
    {
        private readonly WebApiConfiguration _conf;
        private readonly ProxyGenerator _proxyGenerator;
        private readonly Dictionary<string, object> _controllerInstances = new Dictionary<string, object>();

        public WebApiInterceptor(WebApiConfiguration configuration)
        {
            _conf = configuration;
            _proxyGenerator = new Castle.DynamicProxy.ProxyGenerator();
        }

        public void Intercept(IInvocation invocation)
        {
            var isGetAccesor = invocation.Method.DeclaringType != null && invocation.Method.DeclaringType.GetProperties()
               .Any(prop => prop.GetGetMethod() == invocation.Method);

            if (!isGetAccesor)
                throw new NotImplementedException("Only getters accessors are allowed");

            var key = invocation.Method.Name;
            object instance;

            if (_controllerInstances.ContainsKey(key))
                instance = _controllerInstances[key];
            else {
                _controllerInstances.Add(key, instance = _proxyGenerator.CreateInterfaceProxyWithoutTarget(invocation.Method.ReturnType, new WebApiControllerInterceptor(invocation.Method, _conf, GetDelegatinHandlers(invocation.Method))));
            }

            invocation.ReturnValue = instance;
        }

        private DelegatingHandler[] GetDelegatinHandlers(MethodInfo method)
        {
            return method.DeclaringType
                .GetCustomAttributes<DelegatingHandlerAttribute>()
                 .Select(x => x.TypeHandlers)
                .SelectMany(x => x)
                .Select(x => (DelegatingHandler)Activator.CreateInstance(x))
                .ToArray();
        }
    }
}
