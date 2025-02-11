using ApiEasier.Bll.Converters;
using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Bll.Interfaces.ApiEmu;
using ApiEasier.Bll.Interfaces.Converter;
using ApiEasier.Bll.Interfaces.FileWatcher;
using ApiEasier.Bll.Interfaces.Validators;
using ApiEasier.Bll.Services.ApiConfigure;
using ApiEasier.Bll.Services.ApiEmu;
using ApiEasier.Bll.Services.FileWatcher;
using ApiEasier.Bll.Validators;
using ApiEasier.Dal.Data;
using ApiEasier.Dal.Helpers;
using ApiEasier.Dal.Interfaces.Db;
using ApiEasier.Dal.Interfaces.FileStorage;
using ApiEasier.Dal.Interfaces.Helpers;
using ApiEasier.Dal.Repositories.Db;
using ApiEasier.Dal.Repositories.FileStorage;
using ApiEasier.Dm.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ApiEasier.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddMemoryCache();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // DB
            var mongoSettings = builder.Configuration.GetSection("DatabaseSettings");
            string connectionString = mongoSettings["ConnectionString"];
            string databaseName = mongoSettings["DatabaseName"];

            if (connectionString == null || databaseName == null)
                throw new InvalidOperationException("MongoDB settings are missing in appsettings.json");

            builder.Services.AddSingleton<MongoDbContext>(serviceProvider =>
            {
                return new MongoDbContext(connectionString, databaseName);
            });

            // BLL ------------------------------------------
            //Converters
            builder.Services.AddScoped<IConverter<ApiService, ApiServiceDto>, ApiServiceToDtoConverter>();
            builder.Services.AddScoped<IConverter<ApiService, ApiServiceSummaryDto>, ApiServiceToDtoSummaryConverter>();
            builder.Services.AddScoped<IConverter<ApiServiceDto, ApiService>, DtoToApiServiceConverter>();

            builder.Services.AddScoped<IConverter<ApiEntity, ApiEntityDto>, ApiEntityToDtoConverter>();
            builder.Services.AddScoped<IConverter<ApiEntity, ApiEntitySummaryDto>, ApiEntityToDtoSummaryConverter>();
            builder.Services.AddScoped<IConverter<ApiEntityDto, ApiEntity>, DtoToApiEntityConverter>();

            builder.Services.AddScoped<IConverter<ApiEndpoint, ApiEndpointDto>, ApiEndpointToDtoConverter>();
            builder.Services.AddScoped<IConverter<ApiEndpointDto, ApiEndpoint>, DtoToApiEndpointConverter>();

            //Validators
            builder.Services.AddScoped<IDynamicResourceValidator, DynamicResourceValidator>();

            //Services
            builder.Services.AddScoped<IDynamicApiConfigurationService, DynamicApiConfigurationService>();
            builder.Services.AddScoped<IDynamicEntityConfigurationService, DynamicEntityConfigurationService>();
            builder.Services.AddScoped<IDynamicEndpointConfigurationService, DynamicEndpointConfigurationService>();
            builder.Services.AddScoped<IDynamicResourceDataService, DynamicResourceDataService>();
            // ------------------------------------------


            // DAL
            builder.Services.AddScoped<IDbResourceDataRepository, DbResourceDataRepository>();
            // синглтон т.к FileSystemWatcher требует
            builder.Services.AddScoped<IDbResourceRepository, DbResourceRepository>();

            //Helpers
            builder.Services.AddSingleton(sp => new JsonSerializerHelper());

            builder.Services.AddSingleton<IJsonFileHelper, JsonFileHelper>(provider =>
            {
                var jsonDirectoryPath = builder.Configuration["JsonDirectoryPath"] ?? "configuration";
                return new JsonFileHelper(jsonDirectoryPath);
            });
            builder.Services.AddScoped<IFileHelper>(sp => sp.GetRequiredService<IJsonFileHelper>());

            // Repositories
            builder.Services.AddScoped<IFileApiServiceRepository, FileApiServiceRepository>();
            builder.Services.AddScoped<IFileApiEntityRepository, FileApiEntityRepository>();
            builder.Services.AddScoped<IFileApiEndpointRepository, FileApiEndpointRepository>();
            // ------------------------------------------

            //FileSystemWatcherService
            builder.Services.AddScoped<IApiConfigChangeHandler, ApiConfigChangeHandler>();
            builder.Services.AddHostedService(sp =>
            {
                using (var scope = sp.CreateScope())
                {
                    var cache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
                    var apiConfigChangeHandler = scope.ServiceProvider.GetRequiredService<IApiConfigChangeHandler>();
                    var jsonDirectoryPath = builder.Configuration["JsonDirectoryPath"];
                    if (jsonDirectoryPath == null)
                        throw new ArgumentNullException(nameof(jsonDirectoryPath));

                    return new ConfigFileWatcherService(cache, jsonDirectoryPath, apiConfigChangeHandler);
                }
            });


            // Логгирование http в MongoDB
            //builder.Logging.ClearProviders();
            //builder.Services.AddSingleton<ILoggerProvider, MongoLoggerProvider>();
            //builder.Services.AddHttpLogging(o =>
            //{
            //    o.CombineLogs = true;
            //    o.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestQuery;
            //});

            builder.Configuration.AddEnvironmentVariables();

            var app = builder.Build();

            //app.UseHttpLogging();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
