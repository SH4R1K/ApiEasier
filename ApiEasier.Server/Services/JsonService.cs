using ApiEasier.Server.Models;
using System.Text.Json;

namespace ApiEasier.Server.Services
{
    public class JsonService
    {
        private readonly string _path;
        public JsonService(string path)
        {
            _path = path;
            Directory.CreateDirectory(_path);
        }
        public string GetFilePath(string fileName)
        {
            string filePath = Path.Combine(_path, fileName + ".json");

            return filePath;
        }

        public async Task<ApiService?> DeserializeApiServiceAsync(string apiServiceName)
        {
            var filePath = GetFilePath(apiServiceName);
            var json = await File.ReadAllTextAsync(filePath);

            // Десериализация JSON в объект
            var apiService = JsonSerializer.Deserialize<ApiService>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Уменьшение регистра полей
                WriteIndented = true // Запись в читаемом формате
            });
            return apiService;
        }

        public async Task SerializeApiServiceAsync(string fileName, ApiService apiService)
        {
            var filePath = GetFilePath(fileName);
            // Десериализация JSON в объект
            var json = JsonSerializer.Serialize<ApiService>(apiService, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Уменьшение регистра полей
                WriteIndented = true // Запись в читаемом формате
            });

           File.WriteAllText(filePath, json);
        }


        public async Task<ApiEntity?> GetApiEntity(string entityName, string fileName)
        {
            var filePath = GetFilePath(fileName);
            var apiService = await DeserializeApiServiceAsync(filePath);

            var entity = apiService.Entities.FirstOrDefault(e => e.Name == entityName);

            return entity;
        }
    }
}
