using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Proxy.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class NameAttribute : Attribute
    {
        public string Name { get; set; }
        public NameAttribute(string name) { Name = name; }
    }
}
