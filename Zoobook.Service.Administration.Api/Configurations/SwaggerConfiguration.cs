using System;
using System.IO;
using System.Reflection;
using Zoobook.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Zoobook.Service.Administration.Api
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection ConfigureSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(Constants.DocumentVersion, new OpenApiInfo
                {
                    Version = Constants.DocumentVersion,
                    Title = Constants.DocumentTitle,
                    Description = Constants.DocumentDescription
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            });

            return services;
        }
    }
}
