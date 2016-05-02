using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebApi.Proxy.Demo.GoogleMapAPI
{
    [XmlType(Namespace = "")]
    public class GeocodeResponseAddressComponent
    {
        [XmlElement(ElementName = "long_name")]
        public string LongName;
        [XmlElement(ElementName = "short_name")]
        public string ShortName;
        [XmlElement(ElementName = "type")]
        public List<string> Types;
    }
}
