using global::Nacos.V2.DependencyInjection;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNacosV2Naming(x =>
{
    x.ServerAddresses = new System.Collections.Generic.List<string> { "http://localhost:8848/" };
    x.Namespace = "yarp";

    // swich to use http or rpc
    x.NamingUseRpc = true;
});

builder.Services.AddReverseProxy()
    .AddNacosServiceDiscovery(
    groupNames: "DEFAULT_GROUP", 
    percount:100,
    enableAutoRefreshService: true,
    autoRefreshPeriod: 30);

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/yarp", (IProxyConfigProvider provider) =>
    {
        var res = provider.GetConfig();
        return Results.Ok(res);
    });
    endpoints.MapReverseProxy();
});

app.Run("http://*:9091");
