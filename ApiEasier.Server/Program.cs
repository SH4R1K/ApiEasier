using ApiEasier.Bll.Interfaces.ApiEmu;
using ApiEasier.Bll.Services.ApiEmu;
using ApiEasier.Dal.DB;
using Microsoft.AspNetCore.HttpLogging;

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

            // Конфигурация данных для подключения к БД
            builder.Services.AddSingleton<MongoDbContext>(serviceProvider =>
            {
                return new MongoDbContext("mongodb://localhost:27017", "apiEasier");
            });

            // Эумалятор апи сервисов
            builder.Services.AddScoped<IDynamicResource, DynamicResource>();

            // Сервис работы с json-файлами конфигураций api-сервисов
            //builder.Services.AddSingleton<IConfigFileApiService, JsonService>();

            // FileSystemWatcher для отслеживания актуальности кэша
            //builder.Services.AddHostedService<ConfigFileWatcherService>();

            //builder.Services.AddHostedService<TakeOutDbTrashService>();

            // Сервис работы с MongoDB
            //builder.Services.AddSingleton<IDynamicCollectionService, DynamicCollectionService>();

            // Сервис валидации данных переданных emuApi и данных в json-файлах конфигураций api-сервисов
            //builder.Services.AddSingleton<IEmuApiValidationService, EmuApiValidationService>();

            // Логгирование http в MongoDB
            builder.Logging.ClearProviders();
            //builder.Services.AddSingleton<ILoggerProvider, MongoLoggerProvider>();
            builder.Services.AddHttpLogging(o =>
            {
                o.CombineLogs = true;
                o.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestQuery;
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
