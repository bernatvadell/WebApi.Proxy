using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebApi.Proxy.Demo.GoogleMapAPI
{
    [XmlType(Namespace = "")]
    public class GeocodeResponseResult
    {
        [XmlElement(ElementName = "type")]
        public List<string> Types;
        [XmlElement(ElementName = "formatted_address")]
        public string FormattedAddress;
        [XmlElement(ElementName = "address_component")]
        public List<GeocodeResponseAddressComponent> AddressComponents;
        [XmlElement(ElementName = "geometry")]
        public GeocodeResponseResultGeometry Geometry;
    }
}
