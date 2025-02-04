using ApiEasier.Dal.Helpers;
using ApiEasier.Dal.Interfaces.FileStorage;
using ApiEasier.Dm.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiEasier.Dal.Repositories.FileStorage
{
    public class FileApiServiceRepository : IFileApiServiceRepository
    {
        private readonly FileHelper _fileHelper;

        public FileApiServiceRepository(FileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }

        public async Task<bool> CreateAsync(ApiService apiService)
        {
            try
            {
                var filePath = _fileHelper.GetFilePath(apiService.Name);

                var json = JsonHelper.Serialize(apiService);

                await _fileHelper.WriteFileAsync(filePath, json);
                return true;
            }
            catch
            {
                Console.WriteLine($"Ошибка при записи файла");
                return false;
            }

        }

        public bool Delete(string id)
        {
            try
            {
                var filePath = _fileHelper.GetFilePath(id);

                if (!File.Exists(filePath))
                    return false;

                File.Delete(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<ApiService>> GetAllAsync()
        {
            var files = _fileHelper.GetAllFiles();
            var tasks = files.Select(file => GetByIdAsync(Path.GetFileNameWithoutExtension(file)));

            var result = await Task.WhenAll(tasks);
            return result.Where(apiService => apiService != null).ToList();

        }

        public async Task<ApiService?> GetByIdAsync(string id)
        {
            var filePath = _fileHelper.GetFilePath(id);

            if (!File.Exists(filePath))
                return default;

            var json = await _fileHelper.ReadFileAsync(id);

            if (json == null)
                return default;

            var apiService = JsonHelper.Deserialize<ApiService>(json);

            apiService.Name = id;

            return apiService;
        }

        public async Task<ApiService?> UpdateAsync(string id, ApiService apiService)
        {
            try
            {
                var oldfilePath = _fileHelper.GetFilePath(id);
                var newFilePath = _fileHelper.GetFilePath(apiService.Name);

                if (!File.Exists(oldfilePath))
                    return default;

                var jsonOld = await _fileHelper.ReadFileAsync(oldfilePath);

                var newApiService = JsonHelper.Deserialize<ApiService>(jsonOld);

                newApiService.IsActive = apiService.IsActive;
                newApiService.Description = apiService.Description;
                newApiService.Entities = apiService.Entities;

                var json = JsonHelper.Serialize(newApiService);

                File.WriteAllText(newFilePath, json);

                return newApiService;
            }
            catch
            {
                return default;
            }
        }

        public async Task<bool> ChangeActiveStatusAsync(string id, bool status)
        {
            try
            {
                var filePath = _fileHelper.GetFilePath(id);

                if (!File.Exists(filePath))
                    return false;

                var json = await _fileHelper.ReadFileAsync(filePath);

                var newApiService = JsonHelper.Deserialize<ApiService>(json);       

                newApiService.IsActive = status;

                var newJson = JsonHelper.Serialize(newApiService);

                File.WriteAllText(filePath, newJson);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
