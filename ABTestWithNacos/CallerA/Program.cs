using Nacos.V2;
using Nacos.V2.DependencyInjection;
using Nacos.V2.Naming.Dtos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNacosV2Naming(x =>
{
    x.ServerAddresses = new List<string> { "http://localhost:8848/" };
    x.Namespace = "cs";
});

var app = builder.Build();

app.MapGet("/req/{id}", Call);

app.Run("http://*:9884");

async Task<IResult> Call(ILoggerFactory loggerFactory, INacosNamingService svc, IHttpClientFactory factory, int id)
{
    var logger = loggerFactory.CreateLogger(nameof(Call));

    var allIns = await svc.GetAllInstances("providerb", "DEFAULT_GROUP", new List<string> { "DEFAULT" });

    // 按照对应的逻辑做对应的地址获取方式
    // 这里是：id 小于 100 的走新特性
    string address = GetAddress(allIns, id < 100);

    var client = factory.CreateClient();

    var res = await client.GetStringAsync(address);

    logger.LogInformation("user={id},url={url},result={res}", id, address, res);

    return Results.Ok($"caller ------ {res}");
}


string GetAddress(List<Instance> instances, bool isFeature)
{
    var str = isFeature ? "true" : "false";

    var ins = instances
        .Where(x => x.Healthy 
        && x.Enabled 
        && x.Metadata.TryGetValue("feature", out var feature) 
        && feature.Equals(str))
        .OrderBy(x=>Guid.NewGuid())
        .FirstOrDefault();

    return ins != null 
        ? $"http://{ins.Ip}:{ins.Port}" 
        : throw new Exception("Can not find out ins");
}