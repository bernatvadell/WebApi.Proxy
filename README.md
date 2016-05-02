# WebApiProxy
Build your web api client based on an interface

## Install via Nuget

Install this package from NuGet:

> Install-Package WebApi.Proxy.Client

#### My first webapi client with WebApi.Proxy (Example Google Map - Geocode)

We are asumming all web apis have one or more controllers.
Each controller have one o more actions. In this case, we build a container 'GoogleMap' with a controller 'Geocode'.

###### Google Map Container
```
public interface IGoogleMap
{
    [WebApi.Proxy.Attributes.Name("geocode")] // set controller's name, by default is property's name
    IGeocode Geocode { get; }
}
```

###### Geocode Actions
```
public interface IGeocode
{
    [Name("json")] // action's name, by default is property's name
    [MethodType(MethodType.Get)] 
    JObject AsJson(string address, string key);

    [Name("json")]
    [MethodType(MethodType.Get)]
    Task<JObject> AsJsonAsync(string address, string key);
}
```

#### Build client
```
// 1- Build generator and configure it
var generator = new Proxy.ProxyGenerator<IGoogleMapAPI>(new Proxy.WebApiConfiguration
{
    BaseAddress = "http://maps.googleapis.com/maps/api/"
});

// 2- Build API for access it
var api = generator.Build();

var googleApiKey = "";
var address = "1600 Amphitheatre Parkway, Mountain View, CA";

// 3- Call API and get results
var result = api.Geocode.AsJson(address, googleApiKey);
```

#### Url Generation

The geocode's url is:
http://maps.googleapis.com/maps/api/geocode/json?address=<address>&key=<key>

By default, WebApi Proxy build the url with this format:
{baseurl}/{controller}/{action}?{query}

Segments:
BaseUrl: http://maps.googleapis.com/maps/api/
Controller: geocode
Action: json
Query: address=<address>&key=<key>

If you need implements a custom url builder you can create implementation of IUrlBuilder
```
public interface IUrlBuilder
{
    string Build(ActionDefinition action);
}
```

And pass it in the container definition or controller definition with UrlBuilderAttribute
example:
```
[UrlBuilder(typeof(MyCustomUrlBuilder))]
public interface IGoogleMap
{
    [Name("geocode")]
    IGeocode Geocode { get; }
}
```

or if you only apply this UrlBuilder for a specific controller

```
[UrlBuilder(typeof(MyCustomUrlBuilder))]
public interface IGeocode
{
    ...
}
```

#### Add my custom DelegatingHandler
In the WebApiConfiguration you can set the customs DelegatingHandlers
Example:

``` 
var generator = new Proxy.ProxyGenerator<IGoogleMapAPI>(new Proxy.WebApiConfiguration
{
    BaseAddress = "http://maps.googleapis.com/maps/api/",
    DelegatingHandlers = new System.Net.Http.DelegatingHandler[]
    {
        new MyCustomDelegating()
    }
});
```
