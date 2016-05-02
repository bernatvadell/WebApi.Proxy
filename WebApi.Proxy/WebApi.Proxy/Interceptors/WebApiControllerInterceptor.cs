using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApi.Proxy.Attributes;
using WebApi.Proxy.Components;
using WebApi.Proxy.Types;

namespace WebApi.Proxy.Interceptors
{
    public class WebApiControllerInterceptor : IInterceptor
    {
        internal static MediaTypeFormatter DefaultFormatter = new JsonMediaTypeFormatter();
        internal static IUrlEncoder DefaultUrlEncoder = new UrlEncoder();
        internal static IUrlBuilder DefaultUrlBuilder = new UrlBuilder(DefaultUrlEncoder);

        private readonly MethodInfo _method;
        private readonly WebApiConfiguration _conf;
        private readonly ControllerDefinition _controllerDefinition;
        private readonly DelegatingHandler[] _handlers;

        public WebApiControllerInterceptor(MethodInfo method, WebApiConfiguration configuration, DelegatingHandler[] globalHandlers)
        {
            _method = method;
            _conf = configuration;
            _controllerDefinition = GetControllerDefinition();

            _handlers = GetDelegatinHandlers(method)
                .Concat(globalHandlers)
                .Concat(configuration.DelegatingHandlers != null ? configuration.DelegatingHandlers : new DelegatingHandler[0])
                .ToArray();
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.ReturnType == typeof(Task))
            {
                invocation.ReturnValue = ExecuteApi(invocation);
            }
            else if (invocation.Method.ReturnType.IsGenericType && invocation.Method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                invocation.ReturnValue = ExecuteApi(invocation);
            }
            else
            {
                var resultTask = ExecuteApi(invocation);
                resultTask.Wait();
                var getterResult = resultTask.GetType().GetProperty("Result");
                if (getterResult != null)
                    invocation.ReturnValue = getterResult.GetValue(resultTask);
            }
        }

        private ActionDefinition GetActionDefinition(MethodInfo method, object[] arguments)
        {
            var name = method.GetCustomAttribute<NameAttribute>()?.Name ?? method.Name;

            var args = method.GetParameters();
            var methodType = GetMethodType(method);
            arguments = arguments ?? new object[] { };
            var action = new ActionDefinition
            {
                Controller = _controllerDefinition,
                MethodType = methodType,
                Name = name,
                Parameters = (from idx in Enumerable.Range(0, arguments.Length)
                              let arg = args[idx]
                              let val = arguments[idx]
                              select new ParamDefinition
                              {
                                  Name = arg.Name,
                                  Output = GetParamOutput(methodType, arg),
                                  Value = val
                              }).ToList()
            };

            var bodyParameter = action.Parameters.FirstOrDefault(x => x.Output == ParamOutput.Body);
            if (bodyParameter != null)
                action.BodyParameter = bodyParameter.Value;

            return action;
        }

        private MediaTypeFormatter GetFormatter(MemberInfo methodInfo, out string accept)
        {
            accept = null;
            var attribute = methodInfo.GetCustomAttribute<HttpFormatterAttribute>();

            if (attribute != null)
            {
                accept = attribute.Accept;
                return attribute.Formatter;
            }

            if (_conf.DefaultFormatter != null && !string.IsNullOrEmpty(_conf.DefaultAccept))
            {
                accept = _conf.DefaultAccept;
                return _conf.DefaultFormatter;
            }

            return DefaultFormatter;
        }

        private IUrlBuilder GetUrlBuilder(MemberInfo methodInfo)
        {
            var attribute = methodInfo.GetCustomAttribute<UrlBuilderAttribute>();
            if (attribute != null)
                return attribute.UrlBuilder;
            return DefaultUrlBuilder;
        }

        private ControllerDefinition GetControllerDefinition()
        {
            var property = _method.DeclaringType.GetProperties()
                .Where(x => x.GetGetMethod() == _method)
                .FirstOrDefault();

            if (property == null)
                throw new ApplicationException("Only accept controller declared with getter property");

            var name = property.GetCustomAttribute<NameAttribute>()?.Name
                ?? _method.ReturnType.GetCustomAttribute<NameAttribute>()?.Name
                ?? Regex.Replace(_method.ReturnType.Name, "(?:^I|Controller$)", string.Empty, RegexOptions.IgnoreCase);

            return new ControllerDefinition
            {
                Name = name
            };
        }

        private Task ExecuteApi(IInvocation invocation)
        {
            var actionDefinition = GetActionDefinition(invocation.Method, invocation.Arguments);
            var returnType = GetReturnType(invocation);
            var urlBuilder = GetUrlBuilder(invocation.Method);
            var path = urlBuilder.Build(actionDefinition);

            var client = HttpClientFactory.Create(_handlers);
            client.BaseAddress = new Uri(_conf.BaseAddress);

            if (_conf.DefaultRequestHeaders != null)
                foreach (var key in _conf.DefaultRequestHeaders.AllKeys.ToList())
                    client.DefaultRequestHeaders.Add(key, _conf.DefaultRequestHeaders[key]);

            string accept;
            MediaTypeFormatter formatter = GetFormatter(invocation.Method, out accept);

            if (!string.IsNullOrEmpty(accept))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
            }

            var objectTask = HttpClientCall(client, path, formatter, actionDefinition);

            if (returnType != null)
            {
                var task = objectTask.ContinueWith(response =>
                 {
                     response.Result.EnsureSuccessStatusCode();
                     return response.Result.Content.ReadAsAsync(returnType, new MediaTypeFormatter[] { formatter }).Result;
                 }).ContinueWith(x =>
                 {
                     client.Dispose();
                     return x.Result;
                 });

                return CallTask(returnType, task);
            }
            else
            {
                return objectTask
                    .ContinueWith(x =>
                    {
                        client.Dispose();
                    });
            }
        }

        private static Task CallTask(Type returnType, Task<object> objectTask)
        {
            var callTask = typeof(WebApiControllerInterceptor)
                    .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                    .Where(p => p.IsGenericMethod)
                    .FirstOrDefault(p => p.Name.Equals("CallTask", StringComparison.InvariantCultureIgnoreCase));
            if (callTask == null) return null;

            var genericMethod = callTask.MakeGenericMethod(returnType);

            return genericMethod.Invoke(null, new object[] { objectTask }) as Task;
        }

        private static async Task<T> CallTask<T>(Task<object> task)
        {
            return (T)await task;
        }

        private Type GetReturnType(IInvocation invocation)
        {
            if (invocation.Method.ReturnType == typeof(Task) || invocation.Method.ReturnType == typeof(void))
            {
                return null;
            }
            else if (invocation.Method.ReturnType.IsGenericType && invocation.Method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                return invocation.Method.ReturnType.GetGenericArguments()[0];
            }
            else
            {
                return invocation.Method.ReturnType;
            }
        }

        private ParamOutput GetParamOutput(MethodType methodType, ParameterInfo parameterInfo)
        {
            if (methodType == MethodType.Get) return ParamOutput.QueryString;

            var attribute = parameterInfo.GetCustomAttributes<ParamOutputAttribute>().FirstOrDefault();

            if (attribute != null)
                return attribute.ParamOutput;

            return parameterInfo.ParameterType.IsPrimitive ? ParamOutput.QueryString : ParamOutput.Body;
        }

        private MethodType GetMethodType(MethodInfo methodInfo)
        {
            var attribute = methodInfo.GetCustomAttributes<MethodTypeAttribute>().FirstOrDefault();

            if (attribute != null)
                return attribute.MethodType;

            if (methodInfo.Name.IndexOf("Get", StringComparison.InvariantCultureIgnoreCase) > -1)
                return MethodType.Get;
            else if (methodInfo.Name.IndexOf("Create", StringComparison.InvariantCultureIgnoreCase) > -1)
                return MethodType.Post;
            else if (methodInfo.Name.IndexOf("Update", StringComparison.InvariantCultureIgnoreCase) > -1)
                return MethodType.Put;
            else if (methodInfo.Name.IndexOf("Delete", StringComparison.InvariantCultureIgnoreCase) > -1)
                return MethodType.Delete;
            else
                return MethodType.Get;
        }

        /// <summary>
        /// Build an specific request depending on the MethodType
        /// </summary>
        /// <param name="client"></param>
        /// <param name="path"></param>
        /// <param name="actionDefinition"></param>
        /// <returns></returns>
        private Task<HttpResponseMessage> HttpClientCall(HttpClient client, string path, MediaTypeFormatter formatter, ActionDefinition actionDefinition)
        {
            switch (actionDefinition.MethodType)
            {
                case MethodType.Get:
                    return client.GetAsync(path);
                case MethodType.Post:
                    return client.PostAsync(path, actionDefinition.BodyParameter, formatter);
                case MethodType.Put:
                    return client.PutAsync(path, actionDefinition.BodyParameter, formatter);
                case MethodType.Delete:
                    return client.DeleteAsync(path);
                default:
                    return client.GetAsync(path);
            }
        }

        private DelegatingHandler[] GetDelegatinHandlers(MethodInfo method)
        {
            return method.DeclaringType
                .GetCustomAttributes<DelegatingHandlerAttribute>()
                .Select(x => x.TypeHandlers)
                .SelectMany(x => x)
                .Select(x => (DelegatingHandler)Activator.CreateInstance(x))
                .ToArray();
        }
    }
}
