namespace WebApi
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Serilog.Events;

    public class Program
    {
        public static void Main(string[] args)
        {
            var outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}";

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: outputTemplate)
                .CreateLogger();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            try
            {
                Log.ForContext<Program>().Information("Application starting...");
                CreateHostBuilder(args, Log.Logger).Build().Run();
            }
            catch (System.Exception ex)
            {
                Log.ForContext<Program>().Fatal(ex, "Application start-up failed!!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, Serilog.ILogger logger) =>
            Host.CreateDefaultBuilder(args)
                 .ConfigureAppConfiguration((context, builder) =>
                 {
                     var c = builder.Build();                    
                     builder.AddNacosV2Configuration(c.GetSection("NacosConfig"), logAction: x => x.AddSerilog(logger));
                 })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseUrls("http://*:8787");
                })
                .UseSerilog();
    }
}
