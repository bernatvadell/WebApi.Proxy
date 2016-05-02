using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Proxy
{
    public class WebApiConfiguration
    {
        public string BaseAddress { get; set; }
        public long DefaultTimeout { get; set; }
        public string DefaultAccept { get; set; }
        public MediaTypeFormatter DefaultFormatter { get; set; }
        public NameValueCollection DefaultRequestHeaders { get; set; }
        public DelegatingHandler[] DelegatingHandlers { get; set; }

        public WebApiConfiguration()
        {
            DefaultTimeout = 8000;
        }
    }
}
