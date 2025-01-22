using ApiEasier.Server.DB;
using ApiEasier.Server.Interfaces;
using ApiEasier.Server.LogsService;
using ApiEasier.Server.Services;
using Microsoft.AspNetCore.HttpLogging;
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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<DBSettings>(
                builder.Configuration.GetSection("DatabaseSettings")
            );

            builder.Services.AddSingleton<JsonService>(provider => new JsonService("configuration"));
            builder.Services.AddSingleton<MongoDBContext>();
            builder.Services.AddScoped<IDynamicCollectionService, DynamicCollectionService>();
            builder.Services.AddScoped<IEmuApiValidationService, EmuApiValidationService>();

            // Конфигурация логирования в MongoDB
            builder.Logging.ClearProviders();
            builder.Services.AddSingleton<ILoggerProvider, MongoLoggerProvider>();

            builder.Services.AddHttpLogging(o => {
                o.CombineLogs = true;

                o.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestQuery;

                // Если нам понадобиться не все данные логов
                //o.LoggingFields = HttpLoggingFields.RequestQuery
                //    | HttpLoggingFields.RequestMethod
                //    | HttpLoggingFields.RequestPath
                //    | HttpLoggingFields.RequestBody
                //    | HttpLoggingFields.ResponseStatusCode
                //    | HttpLoggingFields.ResponseBody
                //    | HttpLoggingFields.Duration;
            });

            var app = builder.Build();

            app.UseHttpLogging();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
