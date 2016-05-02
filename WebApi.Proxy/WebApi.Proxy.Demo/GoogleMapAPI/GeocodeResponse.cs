using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebApi.Proxy.Demo.GoogleMapAPI
{
    [XmlRoot(ElementName = "GeocodeResponse", Namespace = "")]
    public class GeocodeResponse
    {
        [XmlElement(ElementName = "status")]
        public GeocodeResponseStatusCode Status;
        [XmlElement(ElementName = "result")]
        public List<GeocodeResponseResult> Results;
    }
}
