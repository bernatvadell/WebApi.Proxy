using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Proxy.Demo.GoogleMapAPI
{
    public enum GeocodeResponseStatusCode
    {
        OK,
        ZERO_RESULTS,
        OVER_QUERY_LIMIT,
        REQUEST_DENIED,
        INVALID_REQUEST,
    }
}
