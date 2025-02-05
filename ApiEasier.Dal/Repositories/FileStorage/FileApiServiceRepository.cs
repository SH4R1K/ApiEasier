using ApiEasier.Dal.Helpers;
using ApiEasier.Dal.Interfaces.FileStorage;
using ApiEasier.Dal.Interfaces.Helpers;
using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Repositories.FileStorage
{
    public class FileApiServiceRepository : IFileApiServiceRepository
    {
        private readonly IFileHelper _fileHelper;

        public FileApiServiceRepository(IFileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }

        public async Task<bool> CreateAsync(ApiService apiService)
        {
            try
            {
                await _fileHelper.WriteJsonAsync(apiService.Name, apiService);
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
                var filePath = _fileHelper.DeleteFile(id);
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
            var apiService = await _fileHelper.ReadJsonAsync<ApiService>(id);
            if (apiService == null)
                return null;

            // так как name в json не содержится нужно присовить полю с null значение
            apiService.Name = id;

            return apiService;
        }

        public async Task<ApiService?> UpdateAsync(string id, ApiService apiService)
        {
            try
            {
                var oldApiService = await _fileHelper.ReadJsonAsync<ApiService>(id);

                oldApiService.IsActive = apiService.IsActive;
                oldApiService.Description = apiService.Description;
                oldApiService.Entities = apiService.Entities;


                if (id != apiService.Name)
                    _fileHelper.DeleteFile(id);

                await _fileHelper.WriteJsonAsync(apiService.Name, oldApiService);

                return oldApiService;
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
                var apiService = await _fileHelper.ReadJsonAsync<ApiService>(id);

                apiService.IsActive = status;

                await _fileHelper.WriteJsonAsync(id, apiService);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
