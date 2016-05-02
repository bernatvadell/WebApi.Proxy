using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Proxy.Types
{
    public class ParamDefinition
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public ParamOutput Output { get; set; }
    }
}
