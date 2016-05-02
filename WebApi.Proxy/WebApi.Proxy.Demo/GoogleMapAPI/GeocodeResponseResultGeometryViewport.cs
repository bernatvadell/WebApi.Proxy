using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebApi.Proxy.Demo.GoogleMapAPI
{
    [XmlType(Namespace = "")]
    public class GeocodeResponseResultGeometryViewport
    {
        [XmlElement(ElementName = "southwest")]
        public Location Southwest;
        [XmlElement(ElementName = "northeast")]
        public Location Northeast;
    }
}
