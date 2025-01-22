using ApiEasier.Server.Db;
using ApiEasier.Server.Interfaces;
using ApiEasier.Server.LogsService;
using ApiEasier.Server.Services;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;

namespace ApiEasier.Server
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

            builder.Services.Configure<DbSerttings>(
                builder.Configuration.GetSection("DatabaseSettings")
            );

            builder.Services.AddSingleton<IConfigFileApiService, JsonService>(provider =>
            {
                var memoryCache = provider.GetRequiredService<IMemoryCache>();
                var jsonDirectoryPath = builder.Configuration["JsonDirectoryPath"] ?? "configuration";
                return new JsonService(jsonDirectoryPath, memoryCache);
            });
            builder.Services.AddHostedService(provider =>
            {
                var memoryCache = provider.GetRequiredService<IMemoryCache>();
                var jsonDirectoryPath = builder.Configuration["JsonDirectoryPath"] ?? "configuration";
                return new ConfigFileWatcherService(jsonDirectoryPath, memoryCache);
            });
            builder.Services.AddSingleton<MongoDBContext>();
            builder.Services.AddScoped<IDynamicCollectionService, DynamicCollectionService>();
            builder.Services.AddScoped<IEmuApiValidationService, EmuApiValidationService>();

            // Логгирование в mongoDb
            builder.Logging.ClearProviders();
            builder.Services.AddSingleton<ILoggerProvider, MongoLoggerProvider>();

            builder.Services.AddHttpLogging(o => {
                o.CombineLogs = true;

                o.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestQuery;

                // Если нужны конкретные логи
                //o.LoggingFields = HttpLoggingFields.RequestQuery
                //    | HttpLoggingFields.RequestMethod
                //    | HttpLoggingFields.RequestPath
                //    | HttpLoggingFields.RequestBody
                //    | HttpLoggingFields.ResponseStatusCode
                //    | HttpLoggingFields.ResponseBody
                //    | HttpLoggingFields.Duration;
            });
            builder.Configuration.AddEnvironmentVariables();

            var app = builder.Build();

            app.UseHttpLogging();

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
