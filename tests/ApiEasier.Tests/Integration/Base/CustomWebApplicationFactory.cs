using ApiEasier.Api.DependencyInjections;
using ApiEasier.Dal.Data;
using ApiEasier.Dal.Interfaces;
using ApiEasier.Dal.Interfaces.Helpers;
using ApiEasier.Logger.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mongo2Go;

namespace ApiEasier.Tests.Integration.Base
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        private MongoDbRunner _runner;
        private string _mongoConnectionString;
        private string _testApiConfigurationsPath;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            _runner = MongoDbRunner.Start();
            _mongoConnectionString = _runner.ConnectionString;

            string testDbName = "test_db_" + Guid.NewGuid();

            _testApiConfigurationsPath = Path.Combine("TestApiConfigurations_" + Guid.NewGuid().ToString());

            Directory.CreateDirectory(_testApiConfigurationsPath);

            builder.ConfigureServices(services =>
            {
                // Удаляем регистрацию реальной базы
                services.RemoveAll<MongoDbContext>();

                // Регистрируем MongoDbContext с использованием in-memory MongoDB
                services.AddSingleton(sp => new MongoDbContext(_mongoConnectionString, "test_database"));


                // удаляем регистрацию сервисов, работающих с файлами
                services.RemoveAll(typeof(IFileHelper));
                services.RemoveAll(typeof(IResourceDataRepository));
                services.RemoveAll(typeof(IResourceRepository));
                services.RemoveAll(typeof(IApiServiceRepository));
                services.RemoveAll(typeof(IApiEntityRepository));
                services.RemoveAll(typeof(IApiEndpointRepository));
                services.RemoveAll(typeof(ILoggerService));

                // регистрируем сервисы раюотающие с файлами но уже с тестоым путем
                services.AddDalServices(_testApiConfigurationsPath);
                services.AddHostedServices(_testApiConfigurationsPath);
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _runner?.Dispose();

            if (!string.IsNullOrEmpty(_testApiConfigurationsPath) && Directory.Exists(_testApiConfigurationsPath))
            {
                try
                {
                    Directory.Delete(_testApiConfigurationsPath, true);
                }
                catch { }
            }
        }
    }
}
