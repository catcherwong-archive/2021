using Nacos.AspNetCore.V2;

var builder = WebApplication.CreateBuilder(args);

// nacos server v1.x or v2.x
builder.Services.AddNacosAspNet(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", () =>
    {
        return Results.Ok("order svc 9002");
    });
});

app.Run("http://*:9002");
