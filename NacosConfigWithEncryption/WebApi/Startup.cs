using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Nacos.V2.DependencyInjection;
using System;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddNacosV2Config(Configuration, null, "NacosConfig");
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var configSvc = app.ApplicationServices.GetRequiredService<Nacos.V2.INacosConfigService>();

            var db = $"demo-{DateTimeOffset.Now.ToString("yyyyMMdd_HHmmss")}";

            var oldConfig = "{\"ConnectionStrings\":{\"Default\":\"Server=127.0.0.1;Port=3306;Database=" + db + ";User Id=app;Password=098765;\"},\"version\":\"≤‚ ‘version---\",\"AppSettings\":{\"Str\":\"val\",\"num\":100,\"arr\":[1,2,3,4,5],\"subobj\":{\"a\":\"" + db + "\"}}}";

            configSvc.PublishConfig("demo", "DEFAULT_GROUP", oldConfig).ConfigureAwait(false).GetAwaiter().GetResult();

            var options = app.ApplicationServices.GetRequiredService<IOptionsMonitor<AppSettings>>();

            Console.WriteLine("===”√ IOptionsMonitor ∂¡»°≈‰÷√===");
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(options.CurrentValue));
            Console.WriteLine("");

            Console.WriteLine("===”√ IConfiguration ∂¡»°≈‰÷√===");
            Console.WriteLine(Configuration["ConnectionStrings:Default"]);
            Console.WriteLine("");

            var pwd = $"demo-{new Random().Next(100000, 999999)}";

            var newConfig = "{\"ConnectionStrings\":{\"Default\":\"Server=127.0.0.1;Port=3306;Database="+ db + ";User Id=app;Password="+ pwd +";\"},\"version\":\"≤‚ ‘version---\",\"AppSettings\":{\"Str\":\"val\",\"num\":100,\"arr\":[1,2,3,4,5],\"subobj\":{\"a\":\""+ db +"\"}}}";

            configSvc.PublishConfig("demo", "DEFAULT_GROUP", newConfig).ConfigureAwait(false).GetAwaiter().GetResult();

            System.Threading.Thread.Sleep(500);

            var options2 = app.ApplicationServices.GetRequiredService<IOptionsMonitor<AppSettings>>();

            Console.WriteLine("===”√ IOptionsMonitor ∂¡»°≈‰÷√===");
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(options2.CurrentValue));
            Console.WriteLine("");

            Console.WriteLine("===”√ IConfiguration ∂¡»°≈‰÷√===");
            Console.WriteLine(Configuration["ConnectionStrings:Default"]);
            Console.WriteLine("");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
