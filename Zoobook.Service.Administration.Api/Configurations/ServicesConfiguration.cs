using Zoobook.Core;
using Zoobook.Service.Administration.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Zoobook.Service.Administration.Api
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddSingleton<IUrlSetting, UrlSetting>(
                    config => new UrlSetting()
                    {
                        Base = configuration.GetSection("Url:Base").Value
                    });

            services.AddHttpClient<IEmployeesService, EmployeesService>();

            return services;
        }
    }
}
