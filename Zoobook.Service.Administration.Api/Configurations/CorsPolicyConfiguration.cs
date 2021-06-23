using Zoobook.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Zoobook.Service.Administration.Api
{
    public static class CorsPolicyConfiguration
    {
        public static IServiceCollection ConfigureCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    Constants.ZoobookPolicyName,
                    builder =>
                    {
                        builder.SetIsOriginAllowed((origin) => { return true; });
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                        builder.AllowCredentials();
                    });
            });

            return services;
        }
    }
}
