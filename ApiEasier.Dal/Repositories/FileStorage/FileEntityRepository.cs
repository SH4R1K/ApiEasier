using ApiEasier.Dal.Interfaces.FileStorage;
using ApiEasier.Dm.Models;
using System.Text.Json;

namespace ApiEasier.Dal.Repositories.FileStorage
{
    public class FileEntityRepository : IFileEntityRepository
    {
        public Task<bool> CreateAsync(string apiServiceName, ApiEntity apiEntity)
        {
            throw new NotImplementedException();
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
            var apiService = JsonSerializer.Deserialize<ApiService>(json);

            if (apiService == null)
                return false;

            var entityToRemove = apiService.Entities.FirstOrDefault(e => e.Name == id);
            if (entityToRemove == null)
                return false;

            apiService.Entities.Remove(entityToRemove);

            var updatedJson = JsonSerializer.Serialize(apiService, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await File.WriteAllTextAsync(filePath, updatedJson);

            return true;
        }

        public Task<bool> UpdateAsync(string apiServiceName, string id, ApiEntity apiEntity)
        {
            throw new NotImplementedException();
        }
    }
}
