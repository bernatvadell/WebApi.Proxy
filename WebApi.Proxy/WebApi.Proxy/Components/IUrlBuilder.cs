using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Proxy.Types;

namespace WebApi.Proxy.Components
{
    public interface IUrlBuilder
    {
        string Build(ActionDefinition action);
    }
}
