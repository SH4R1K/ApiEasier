using ApiEasier.Dal.Interfaces.FileStorage;
using ApiEasier.Dm.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiEasier.Dal.Repositories.FileStorage
{
    public class FileEntityRepository : IFileEntityRepository
    {
        public async Task<bool> CreateAsync(string apiServiceName, ApiEntity apiEntity)
        {
            var filePath = GetFilePath(apiServiceName);

            if (!File.Exists(filePath))
                return false;


        }

        private readonly string _folderPath;

        public FileEntityRepository(string folderPath)
        {
            _folderPath = folderPath;
        }

        private string GetFilePath(string fileName)
        {
            return Path.Combine(_folderPath, fileName + ".json");
        }

        public async Task<bool> DeleteAsync(string apiServiceName, string id)
        {
            var filePath = GetFilePath(apiServiceName);

            if (!File.Exists(filePath))
                return false;

            var json = await File.ReadAllTextAsync(filePath);
            var apiService = JsonSerializer.Deserialize<ApiService>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            if (apiService == null)
                return false;

            var entityToRemove = apiService.Entities.FirstOrDefault(e => e.Name == id);
            if (entityToRemove == null)
                return false;

            apiService.Entities.Remove(entityToRemove);

            var updatedJson = JsonSerializer.Serialize(apiService, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            await File.WriteAllTextAsync(filePath, updatedJson);

            return true;
        }

        public async Task<bool> UpdateAsync(string apiServiceName, string id, ApiEntity apiEntity)
        {
            try
            {
                var filePath = GetFilePath(apiServiceName);

                if (!File.Exists(filePath))
                    return false;

                
            }
            catch
            {
                return default;
            }
        }
    }
}
