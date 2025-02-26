using ApiEasier.Api.DependencyInjections;
using ApiEasier.Bll.Middleware.Logger;

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
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var basePath = AppContext.BaseDirectory;

                var xmlPath = Path.Combine(basePath, "ApiEasier.Api.xml");
                options.IncludeXmlComments(xmlPath);
            });


            var apiConfigurationsPath = "ApiConfigurations";

            // Убедимся, что папка существует
            if (!Directory.Exists(apiConfigurationsPath))
                Directory.CreateDirectory(apiConfigurationsPath);

            // Регистрация зависимостей через методы расширения
            builder.Services.AddDatabase(builder.Configuration)
                            .AddBllServices()
                            .AddDalServices(apiConfigurationsPath)
                            .AddHostedServices(apiConfigurationsPath);

            builder.Configuration.AddEnvironmentVariables();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseMiddleware<HttpLoggingMiddleware>();

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
