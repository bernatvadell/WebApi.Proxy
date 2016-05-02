using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Proxy.Types;

namespace WebApi.Proxy.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class MethodTypeAttribute : Attribute
    {
        public MethodType MethodType { get; set; }

        public MethodTypeAttribute(MethodType methodType)
        {
            MethodType = methodType;
        }
    }
}
