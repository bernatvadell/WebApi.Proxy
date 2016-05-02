using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebApi.Proxy.Demo.GoogleMapAPI
{
    [XmlType(Namespace = "")]
    public class Location
    {
        [XmlElement(ElementName = "lat")]
        public string Lat;
        [XmlElement(ElementName = "lng")]
        public string Lng;
    }
}
