using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Proxy.Attributes;

namespace WebApi.Proxy.Demo.GoogleMapAPI
{
    public interface IGoogleMap
    {
        [Name("geocode")]
        IGeocode Geocode { get; }
    }
}
