using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Zoobook.Service.Administration.Api
{
    public static class ResponseCompressionConfiguration
    {
        public static IServiceCollection ConfigureResponseCompression(this IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = new[]
                {
                    "text/plain",
                    "text/html",
                    "application/xml",
                    "text/xml",
                    "application/json",
                    "text/json",
                };
            });

            return services;
        }
    }
}
