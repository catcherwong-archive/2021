using Nacos.AspNetCore.V2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddNacosAspNet(x =>
{
    x.ServerAddresses = new List<string> { "http://localhost:8848/" };
    x.Namespace = "cs";

    // keep lowecase here will be better
    x.ServiceName = "sample";
    x.GroupName = "Some_Group";
    x.ClusterName = "DC_1";
    x.Weight = 100;
    x.Secure = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/api/get", () =>
{
    return Results.Ok("from .net6 minimal API");
});

app.Run("http://*:9991");