using ApiEasier.Domain.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiEasier.Infrastructure.Repositories
{
    public class JsonRepository : IJsonRepository
    {
        private readonly string _baseFolderPath;

        public JsonRepository(string baseFolderPath)
        {
            _baseFolderPath = baseFolderPath;
        }

        private string GetFilePath(string fileName)
        {
            return Path.Combine(_baseFolderPath, fileName + ".json");
        }

        public async Task<T?> ReadJsonAsync<T>(string fileName)
        {
            var filePath = GetFilePath(fileName);

            if (!File.Exists(filePath))
                return default;

            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        }

        public async Task WriteJsonAsync<T>(string fileName, T data)
        {
            var filePath = GetFilePath(fileName);

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            await File.WriteAllTextAsync(filePath, json);
        }
    }
}
