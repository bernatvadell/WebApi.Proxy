using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Proxy.Types;

namespace WebApi.Proxy.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public class ParamOutputAttribute : Attribute
    {
        public ParamOutput ParamOutput { get; set; }

        public ParamOutputAttribute(ParamOutput paramOutput)
        {
            ParamOutput = paramOutput;
        }
    }
}
