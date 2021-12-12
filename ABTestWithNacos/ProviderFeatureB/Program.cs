using Nacos.AspNetCore.V2;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNacosAspNet(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () =>
{
    return Results.Ok("OK - feature");
});

app.Run("http://*:9885");
