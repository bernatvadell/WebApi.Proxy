using System.Linq;
using System.Text;
using WebApi.Proxy.Types;

namespace WebApi.Proxy.Components
{
    public class UrlBuilder : IUrlBuilder
    {
        private readonly IUrlEncoder _urlEncoder;

        public UrlBuilder(IUrlEncoder urlEncoder)
        {
            _urlEncoder = urlEncoder;
        }

        public string Build(ActionDefinition action)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}/{1}", action.Controller.Name, action.Name);

            var qsp = action.Parameters.Where(p => p.Output == ParamOutput.QueryString)
                .Select(p => _urlEncoder.GetOutputParams(p.Value, p.Name))
                .SelectMany(p => p as string[] ?? p.ToArray())
                .ToList();

            var qs = string.Join("&", qsp);

            if (!string.IsNullOrEmpty(qs))
                sb.Append("?").Append(qs);

            return sb.ToString();
        }
    }
}
