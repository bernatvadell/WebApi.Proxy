using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Proxy.Types
{
    public class ControllerDefinition
    {
        public IEnumerable<Attribute> Attributes { get; set; }
        public string Name { get; set; }
    }
}
