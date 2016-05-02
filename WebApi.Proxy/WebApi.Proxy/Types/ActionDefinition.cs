using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using WebApi.Proxy.Components;

namespace WebApi.Proxy.Types
{
    public class ActionDefinition
    {
        public string Name { get; set; }
        public MethodType MethodType { get; set; }
        public List<ParamDefinition> Parameters { get; set; }
        public ControllerDefinition Controller { get; set; }
        public object BodyParameter { get; set; }
    }
}
