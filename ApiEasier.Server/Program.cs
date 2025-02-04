using ApiEasier.Bll.Interfaces.ApiEmu;
using ApiEasier.Bll.Services.ApiEmu;
using ApiEasier.Dal.Data;
using ApiEasier.Dal.Interfaces.Db;
using ApiEasier.Dal.Interfaces.File;
using ApiEasier.Dal.Repositories.Db;
using ApiEasier.Dal.Repositories.File;
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


            // DB
            var mongoSettings = builder.Configuration.GetSection("MongoDb");
            string connectionString = mongoSettings["ConnectionString"];
            string databaseName = mongoSettings["DatabaseName"];

            if (connectionString == null || databaseName == null)
                throw new InvalidOperationException("MongoDB settings are missing in appsettings.json");

            builder.Services.AddSingleton<MongoDbContext>(serviceProvider =>
            {
                return new MongoDbContext(connectionString, databaseName);
            });

            // BLL
            // ApiEmu
            builder.Services.AddScoped<IDynamicResourceDataService, DynamicResourceDataService>();
            builder.Services.AddScoped<IValidatorDynamicApiService, ValidatorDynamicApiService>();
            //ApiConfigure

            // DAL
            builder.Services.AddScoped<IDbResourceDataRepository, DbResourceDataRepository>();
            builder.Services.AddScoped<IDbResourceRepository, DbResourceRepository>();
            builder.Services.AddScoped<IFileApiServiceRepository, FileApiServiceRepository>();




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
