using ApiEasier.Server.DB;
using ApiEasier.Server.Interfaces;
using ApiEasier.Server.Services;
using MongoDB.Driver;

namespace ApiEasier.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // �������� ������������ �� ������ � ����� "configuration"
            LoadConfigurationsFromFolder(builder.Configuration, "configuration");

            builder.Services.AddSingleton<JsonService>();
            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<DBSettings>(
                builder.Configuration.GetSection("DatabaseSettings")
            );

            builder.Services.AddSingleton<MongoDBContext>();

            builder.Services.AddScoped<IDynamicCollectionService, DynamicCollectionService>();

            var app = builder.Build();

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

        private static void LoadConfigurationsFromFolder(IConfigurationBuilder configurationBuilder, string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                var jsonFiles = Directory.GetFiles(folderPath, "*.json");
                foreach (var file in jsonFiles)
                {
                    configurationBuilder.AddJsonFile(file, optional: true, reloadOnChange: true);
                }
            }
        }
    }
}
