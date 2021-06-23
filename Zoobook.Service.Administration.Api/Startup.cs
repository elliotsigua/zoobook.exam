using Zoobook.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Zoobook.Service.Administration.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureLogging()
                    .ConfigureControllers()
                    .ConfigureAutoMapper()
                    .ConfigureCorsPolicy()
                    .ConfigureSwaggerDocumentation()
                    .ConfigureServices(Configuration)
                    .ConfigureDatabase(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(action => action.RouteTemplate = "docs/{documentName}/swagger.json");
            app.UseSwaggerUI(action =>
            {
                action.RoutePrefix = "docs";
                action.SwaggerEndpoint($"/docs/{Constants.DocumentVersion}/swagger.json", Constants.DocumentVersion);
            });

            app.UseCors(Constants.ZoobookPolicyName);
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
