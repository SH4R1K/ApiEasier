using ApiEasier.Api.HostedServices;
using ApiEasier.Bll.Converters;
using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Bll.Interfaces.ApiEmu;
using ApiEasier.Bll.Interfaces.Converter;
using ApiEasier.Bll.Interfaces.DbDataCleanup;
using ApiEasier.Bll.Interfaces.Logger;
using ApiEasier.Bll.Interfaces.Validators;
using ApiEasier.Bll.Services.ApiConfigure;
using ApiEasier.Bll.Services.ApiEmu;
using ApiEasier.Bll.Services.DbDataCleanup;
using ApiEasier.Bll.Services.Logger;
using ApiEasier.Bll.Validators;
using ApiEasier.Dal.Data;
using ApiEasier.Dal.Helpers;
using ApiEasier.Dal.Interfaces;
using ApiEasier.Dal.Interfaces.Helpers;
using ApiEasier.Dal.Repositories.Db;
using ApiEasier.Dal.Repositories.FileStorage;
using ApiEasier.Dm.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ApiEasier.Api.DependensyInjections
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoSettings = configuration.GetSection("DatabaseSettings");
            string connectionString = mongoSettings["ConnectionString"];
            string databaseName = mongoSettings["DatabaseName"];

            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(databaseName))
                throw new InvalidOperationException("MongoDB settings are missing in appsettings.json");

            services.AddSingleton(sp => new MongoDbContext(connectionString, databaseName));
            return services;
        }

        public static IServiceCollection AddBllServices(this IServiceCollection services)
        {
            //Converters
            services.AddScoped<IConverter<ApiService, ApiServiceDto>, ApiServiceToDtoConverter>();
            services.AddScoped<IConverter<ApiService, ApiServiceSummaryDto>, ApiServiceToDtoSummaryConverter>();
            services.AddScoped<IConverter<ApiServiceDto, ApiService>, DtoToApiServiceConverter>();
            services.AddScoped<IConverter<ApiEntity, ApiEntityDto>, ApiEntityToDtoConverter>();
            services.AddScoped<IConverter<ApiEntity, ApiEntitySummaryDto>, ApiEntityToDtoSummaryConverter>();
            services.AddScoped<IConverter<ApiEntityDto, ApiEntity>, DtoToApiEntityConverter>();
            services.AddScoped<IConverter<ApiEndpoint, ApiEndpointDto>, ApiEndpointToDtoConverter>();
            services.AddScoped<IConverter<ApiEndpointDto, ApiEndpoint>, DtoToApiEndpointConverter>();

            //Validators
            services.AddScoped<IDynamicResourceValidator, DynamicResourceValidator>();

            //Services
            services.AddScoped<IDynamicApiConfigurationService, DynamicApiConfigurationService>();
            services.AddScoped<IDynamicEntityConfigurationService, DynamicEntityConfigurationService>();
            services.AddScoped<IDynamicEndpointConfigurationService, DynamicEndpointConfigurationService>();
            services.AddScoped<IDynamicResourceDataService, DynamicResourceDataService>();

            //DbDataCleanupService
            services.AddScoped<IDbDataCleanupService, DbDataCleanupService>();

            return services;
        }

        public static IServiceCollection AddDalServices(this IServiceCollection services, string apiConfigurationsPath)
        {
            services.AddScoped<IResourceDataRepository, DbResourceDataRepository>();
            services.AddScoped<IResourceRepository, DbResourceRepository>();

            services.AddScoped<IFileHelper, JsonFileHelper>(provider =>
            {
                var memoryCache = provider.GetRequiredService<IMemoryCache>();

                return new JsonFileHelper(apiConfigurationsPath, memoryCache);
            });

            // Repositories
            services.AddScoped<IApiServiceRepository, FileApiServiceRepository>();
            services.AddScoped<IApiEntityRepository, FileApiEntityRepository>();
            services.AddScoped<IApiEndpointRepository, FileApiEndpointRepository>();
            // ------------------------------------------

            services.AddSingleton<ILoggerService, NLogService>();

            return services;
        }

        public static IServiceCollection AddHostedServices(this IServiceCollection services, string apiConfigurationsPath)
        {
            services.AddHostedService(sp =>
            {
                using (var scope = sp.CreateScope())
                {
                    var cache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();

                    return new ConfigFileWatcherService(cache, apiConfigurationsPath);
                }
            });


            services.AddHostedService(sp =>
            {
                using (var scope = sp.CreateScope())
                {
                    var dbDataCleanupService = scope.ServiceProvider.GetRequiredService<IDbDataCleanupService>();
                    var logger = scope.ServiceProvider.GetRequiredService<ILoggerService>();

                    return new DataCleanupService(dbDataCleanupService, logger);
                }
            });

            return services;
        }
    }
}
