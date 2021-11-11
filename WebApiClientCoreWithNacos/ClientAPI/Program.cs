var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddNacosV2Naming(x =>
{
    x.ServerAddresses = new List<string> { "http://localhost:8848/" };
    x.Namespace = "cs";
});

// builder.Services.AddNacosDiscoveryTypedClient<ISampleApi>("Some_Group", "DC_1");
builder.Services.AddNacosDiscoveryTypedClient<ISampleApi>(x => 
{
    // HttpApiOptions
    x.UseLogging = true;
}, "Some_Group", "DC_1");

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/", async (ISampleApi api) =>
{
    var res = await api.GetAsync();
    return $"client ===== {res}" ;
});

app.Run("http://*:9992");
