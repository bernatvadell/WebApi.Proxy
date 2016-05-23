using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Proxy.Demo.GoogleMapAPI;

namespace WebApi.Proxy.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1- Build generator and configure it
            var generator = new Proxy.ProxyGenerator<IGoogleMap>(new WebApiConfiguration
            {
                BaseAddress = "http://maps.googleapis.com/maps/api/"
            });

            // 2- Build API for access it
            var api = generator.Build();

            var googleApiKey = "";
            var address = "1600 Amphitheatre Parkway, Mountain View, CA";

            // 3- Call API and get results
            var result = api.Geocode.AsJson(address, googleApiKey);
            var result1 = api.Geocode.AsJsonPost(new GeocodeRequest
            {
                Address = address,
                Key = googleApiKey
            });
            var result2 = api.Geocode.AsJsonAsync(address, googleApiKey).Result;
            var result3 = api.Geocode.AsXml(address, googleApiKey);
            var result4 = api.Geocode.AsXmlAsync(address, googleApiKey).Result;

        }
    }
}
