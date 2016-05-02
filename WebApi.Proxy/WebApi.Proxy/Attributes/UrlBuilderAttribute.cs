using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Proxy.Components;

namespace WebApi.Proxy.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class UrlBuilderAttribute : Attribute
    {
        public IUrlBuilder UrlBuilder { get; set; }

        public UrlBuilderAttribute(Type urlBuilderType)
        {
            UrlBuilder = (IUrlBuilder)Activator.CreateInstance(urlBuilderType);
        }
    }
}
