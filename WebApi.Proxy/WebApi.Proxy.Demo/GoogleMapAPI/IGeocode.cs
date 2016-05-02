using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Proxy.Attributes;
using WebApi.Proxy.Demo.Serializers;
using WebApi.Proxy.Types;

namespace WebApi.Proxy.Demo.GoogleMapAPI
{
    public interface IGeocode
    {
        [Name("json")]
        [MethodType(MethodType.Get)]
        JObject AsJson(string address, string key);

        [Name("json")]
        [MethodType(MethodType.Get)]
        Task<JObject> AsJsonAsync(string address, string key);

        [Name("xml")]
        [HttpFormatter("text/xml", typeof(XmlSerializerMediaTypeFormatter))]
        [MethodType(MethodType.Get)]
        GeocodeResponse AsXml(string address, string key);

        [Name("xml")]
        [HttpFormatter("text/xml", typeof(XmlSerializerMediaTypeFormatter))]
        [MethodType(MethodType.Get)]
        Task<GeocodeResponse> AsXmlAsync(string address, string key);
    }
}
