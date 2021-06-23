using AutoMapper;
using LightInject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using Zoobook.Core;
using Zoobook.Service.Administration.DataLayer;
using Zoobook.Service.Administration.Domain;

namespace Zoobook.Test.Domain.Administration
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            // Mock Mapper
            var config = new MapperConfiguration(cfg => cfg.AddProfile(typeof(MockMappingProfile)));
            serviceRegistry.Register(c => config.CreateMapper());

            // Mock Logger
            serviceRegistry.Register(typeof(ILogger<>), typeof(MockLogger<>));

            // Mock Url Settings
            serviceRegistry.Register<IUrlSetting>(config => new UrlSetting() { Base = MockConstants.ApiBaseUrl }, new PerRequestLifeTime());

            // Mock Http Client
            serviceRegistry.Register(ctx => {
                var urlSetting = ctx.GetInstance<IUrlSetting>();
                return new HttpClient() { BaseAddress = new Uri(urlSetting.Base) };
            }, new PerRequestLifeTime());

            // Mock Db Context
            var options = new DbContextOptionsBuilder<ZoobookContextBase>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            var context = new ZoobookAdministrationDbContext(options);
            serviceRegistry.Register(_ => context);

            // Service
            serviceRegistry.Register<IEmployeesService, EmployeesService>();

            // Create Mock Data
            MockEmployeeData.Instance().CreateMockData(context);
        }
    }
}
