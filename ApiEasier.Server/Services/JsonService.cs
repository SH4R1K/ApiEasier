using ApiEasier.Server.Models;
using System.Text.Json;

namespace ApiEasier.Server.Services
{
    public class JsonService
    {
        public string GetFilePath(string fileName)
        {
            // Определение пути к файлу
            string directoryPath = "configuration";
            string filePath = Path.Combine(directoryPath, fileName + ".json");

            // Создание папки, если она не существует
            Directory.CreateDirectory(directoryPath);

            return filePath;
        }

        public async Task<ApiService?> DeserializeApiServiceAsync(string filePath)
        {
            // Чтение содержимого файла
            var json = await System.IO.File.ReadAllTextAsync(filePath);

            // Десериализация JSON в объект
            var apiService = JsonSerializer.Deserialize<ApiService>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Уменьшение регистра полей
                WriteIndented = true // Запись в читаемом формате
            });
            return apiService;
        }

        public async Task SerializeApiServiceAsync(string filePath, ApiService apiService)
        {
            // Десериализация JSON в объект
            var json = JsonSerializer.Serialize<ApiService>(apiService, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Уменьшение регистра полей
                WriteIndented = true // Запись в читаемом формате
            });

            System.IO.File.WriteAllText(filePath, json);
        }


        public async Task<ApiEntity?> GetApiEntity(string entityName, string filePath)
        {
            var apiService = await DeserializeApiServiceAsync(filePath);

            var entity = apiService.Entities.FirstOrDefault(e => e.Name == entityName);

            return entity;
        }
    }
}
