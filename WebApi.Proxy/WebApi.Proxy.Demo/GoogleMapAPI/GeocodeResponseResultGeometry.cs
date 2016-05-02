using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebApi.Proxy.Demo.GoogleMapAPI
{
    [XmlType(Namespace = "")]
    public class GeocodeResponseResultGeometry
    {
        [XmlElement(ElementName = "location")]
        public Location Location;
        [XmlElement(ElementName = "location_type")]
        public GeocodeResponseResultGeometryLocationType LocationType;
        [XmlElement(ElementName = "viewport")]
        public GeocodeResponseResultGeometryViewport Viewport;
    }
}
