using ApiEasier.Api.DependensyInjections;
using ApiEasier.Bll.Services.Logger;

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
