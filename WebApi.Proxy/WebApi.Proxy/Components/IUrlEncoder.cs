using System.Collections.Generic;

namespace WebApi.Proxy.Components
{
    public interface IUrlEncoder
    {
        IEnumerable<string> GetOutputParams(object obj, string path);
    }
}
