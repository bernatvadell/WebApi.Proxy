using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Proxy.Attributes
{
    [AttributeUsage(AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    public class DelegatingHandlerAttribute : Attribute
    {
        public Type[] TypeHandlers { get; set; }

        public DelegatingHandlerAttribute(params Type[] delegatingHandlers)
        {
            if (delegatingHandlers == null || !delegatingHandlers.All(x => typeof(DelegatingHandler).IsAssignableFrom(x)))
                throw new ArgumentException("Invalid delegatingHandlers", "delegatingHandlers");
            TypeHandlers = delegatingHandlers;
        }
    }
}
