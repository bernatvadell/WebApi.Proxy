using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Proxy.Components
{
    public class UrlEncoder : IUrlEncoder
    {
        private readonly Type[] _primitiveTypes = { typeof(decimal), typeof(string) };

        private string EncodeUriValue(object value)
        {
            return value == null ? string.Empty : Uri.EscapeDataString(value.ToString());
        }

        private string GetQueryParam(string name, object value)
        {
            return string.Format("{0}={1}", name, EncodeUriValue(value));
        }

        public IEnumerable<string> GetOutputParams(object obj, string path)
        {
            if (obj == null) return Enumerable.Empty<string>();
            var output = new List<string>();

            var objType = obj.GetType();
            var props = objType.GetProperties();

            if (objType.IsPrimitive || _primitiveTypes.Any(b => b == objType) || props.Length == 0)
            {
                output.Add(GetQueryParam(path, obj));
            }
            else if (typeof(IEnumerable).IsAssignableFrom(objType))
            {
                var collection = (IEnumerable)obj;
                var idx = 0;
                foreach (var item in collection)
                {
                    var arrayPath = string.Format("{0}[{1}]", path, idx++);
                    var subParameters = GetOutputParams(item, arrayPath);
                    output.AddRange(subParameters);
                }
            }
            else
            {
                foreach (var p in props)
                {
                    var propertyPath = string.Concat(path, ".", p.Name);
                    var subParameters = GetOutputParams(p.GetValue(obj), propertyPath);
                    output.AddRange(subParameters);
                }
            }

            return output.AsEnumerable();
        }
    }
}
