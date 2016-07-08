using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Proxy.Attributes
{
    public class RouteAttribute : Attribute
    {
        public bool IsRest { get; set; }
    }
}
