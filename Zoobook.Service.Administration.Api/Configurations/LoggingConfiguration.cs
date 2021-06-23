using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Zoobook.Service.Administration.Api
{
    public static class LoggingConfiguration
    {
        public static IServiceCollection ConfigureLogging(this IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddFilter("Microsoft", LogLevel.Warning);
                builder.AddFilter("System", LogLevel.Error);
                builder.AddFilter("Engine", LogLevel.Warning);
            });
            return services;
        }
    }
}
