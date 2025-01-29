using ApiEasier.Dal.Interfaces;
using ApiEasier.Dm.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiEasier.Dal.Repositories
{
    public class FileApiServiceRepository : IFileApiServiceRepository
    {
        private readonly string _folderPath;

        public FileApiServiceRepository(string folderPath)
        {
            _folderPath = folderPath;
        }

        private string GetFilePath(string fileName)
        {
            return Path.Combine(_folderPath, fileName + ".json");
        }

        public async Task AddAsync(ApiService apiService)
        {
            var filePath = GetFilePath(apiService.Name);

            var json = JsonSerializer.Serialize(apiService, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            await File.WriteAllTextAsync(filePath, json);
        }

        public void Delete(ApiService apiService)
        {
            File.Delete(GetFilePath(apiService.Name));
        }

        public async Task<List<ApiService>> GetAllAsync()
        {
            DirectoryInfo directory = new DirectoryInfo(_folderPath);
            var files = directory.GetFiles("*.json");
            List<ApiService> result = new List<ApiService>();

            foreach (var file in files)
            {
                result.Add(await GetByIdAsync(file.Name));
            }
            
            return result;
        }

        public async Task<ApiService?> GetByIdAsync(string id)
        {
            var filePath = GetFilePath(id);

            if (!File.Exists(filePath))
                return default;

            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<ApiService>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        }

        public async Task UpdateAsync(string id, ApiService apiService)
        {
            var oldfilePath = GetFilePath(id);
            var newFilePath = GetFilePath(apiService.Name);

            if (!File.Exists(oldfilePath))
                return;

            var jsonOld = await File.ReadAllTextAsync(oldfilePath);

            var newApiService = JsonSerializer.Deserialize<ApiService>(jsonOld, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            newApiService.IsActive = apiService.IsActive;
            newApiService.Description = apiService.Description;
            newApiService.Entities = apiService.Entities;

            var json = JsonSerializer.Serialize(newApiService, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            File.WriteAllText(newFilePath, json);
        }
    }
}
