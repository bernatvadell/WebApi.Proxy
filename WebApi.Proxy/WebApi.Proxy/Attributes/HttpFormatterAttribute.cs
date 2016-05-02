using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Proxy.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class HttpFormatterAttribute : Attribute
    {
        public string Accept { get; set; }
        public MediaTypeFormatter Formatter { get; set; }

        public HttpFormatterAttribute(string accept, Type formatter)
        {
            Formatter = (MediaTypeFormatter)Activator.CreateInstance(formatter);
            Accept = accept;
        }
    }
}
