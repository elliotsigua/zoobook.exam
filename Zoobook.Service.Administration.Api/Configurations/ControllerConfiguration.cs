using Zoobook.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Zoobook.Service.Administration.Api
{
    public static class ControllerConfiguration
    {
        public static IServiceCollection ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                        SerializerUtility.GetJsonSerializerOptions(options.JsonSerializerOptions)); ;
            return services;
        }
    }
}
