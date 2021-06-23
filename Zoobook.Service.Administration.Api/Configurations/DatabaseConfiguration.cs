using Zoobook.Core;
using Zoobook.Service.Administration.DataLayer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Zoobook.Service.Administration.Api
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddSingleton<IDatabaseSetting, DatabaseSetting>(
                    config => new DatabaseSetting()
                    {
                        Name = configuration.GetSection("Database:Name").Value,
                        Host = configuration.GetSection("Database:Host").Value,
                        User = configuration.GetSection("Database:User").Value,
                        Password = configuration.GetSection("Database:Password").Value,
                        Port = uint.Parse(configuration.GetSection("Database:Port").Value),
                        Pooling = bool.Parse(configuration.GetSection("Database:Pooling").Value)
                    }
                );

            services
                .AddTransient(provider =>
                 {
                     var databaseSetting = provider.GetRequiredService<IDatabaseSetting>();
                     return new ZoobookAdministrationDbContext(databaseSetting);
                 });

            services
                .BuildServiceProvider()
                .GetService<ZoobookAdministrationDbContext>()
                .UpdateDatabaseMigration();

            return services;
        }
    }
}
