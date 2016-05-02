using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Proxy.Demo.Serializers
{
    public class XmlSerializerMediaTypeFormatter : XmlMediaTypeFormatter
    {
        public XmlSerializerMediaTypeFormatter()
        {
            UseXmlSerializer = true;
        }
    }
}
