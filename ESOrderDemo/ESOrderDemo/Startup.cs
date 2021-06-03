namespace ESOrderDemo
{
    using Elasticsearch.Net;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Nest;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IElasticClient>(x =>
            {
                var uris = new List<Uri> { new Uri("http://localhost:9200") };
                var pool = new StaticConnectionPool(uris);
                var settings = new ConnectionSettings(pool);
#if DEBUG
                settings.DisableDirectStreaming(true);
#endif

                var client = new ElasticClient(settings);
                return client;
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ESOrderDemo", Version = "v1" });

                var projectName = Assembly.GetEntryAssembly().GetName().Name;

                var webXmlFile = $"{projectName}.xml";
                var webXmlPath = Path.Combine(AppContext.BaseDirectory, webXmlFile);

                if (File.Exists(webXmlPath))
                    c.IncludeXmlComments(webXmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ESOrderDemo v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
