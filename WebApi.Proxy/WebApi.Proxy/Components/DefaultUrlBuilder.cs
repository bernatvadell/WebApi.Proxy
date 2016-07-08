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
            var isRest = action.Attributes
                .OfType<Attributes.RouteAttribute>()
                .Concat(action.Controller.Attributes.OfType<Attributes.RouteAttribute>())
                .Where(x => x.IsRest)
                .Any();

            var sb = new StringBuilder();

            if (isRest)
            {
                var restId = action.Parameters.Where(x => x.Output == ParamOutput.RestId).FirstOrDefault();

                if (restId != null)
                    sb.AppendFormat("{0}/{1}", action.Controller.Name, restId.Value);
                else
                    sb.AppendFormat("{0}", action.Controller.Name);
            }
            else
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
