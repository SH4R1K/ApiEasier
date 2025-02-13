using ApiEasier.Api.HostedServices;
using ApiEasier.Bll.Converters;
using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Bll.Interfaces.ApiEmu;
using ApiEasier.Bll.Interfaces.Converter;
using ApiEasier.Bll.Interfaces.Logger;
using ApiEasier.Bll.Interfaces.DbDataCleanup;
using ApiEasier.Bll.Interfaces.Validators;
using ApiEasier.Bll.Services.ApiConfigure;
using ApiEasier.Bll.Services.ApiEmu;
using ApiEasier.Bll.Services.DbDataCleanup;
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
using ApiEasier.Bll.Services.Logger;
using ApiEasier.Api.DependensyInjections;

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


            var apiConfigurationsPath = "ApiConfigurations";

            // Убедимся, что папка существует
            if (!Directory.Exists(apiConfigurationsPath))
                Directory.CreateDirectory(apiConfigurationsPath);

            // Регистрация зависимостей через методы расширения
            builder.Services.AddDatabase(builder.Configuration)
                            .AddBllServices()
                            .AddDalServices(apiConfigurationsPath)
                            .AddHostedServices(apiConfigurationsPath);

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
            app.UseMiddleware<HttpLoggingMiddleware>();
            app.UseMiddleware<ExceptionHandlerMiddleware>();

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
