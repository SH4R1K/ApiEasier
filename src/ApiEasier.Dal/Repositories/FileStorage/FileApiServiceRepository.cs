using ApiEasier.Dal.Interfaces;
using ApiEasier.Dal.Interfaces.Helpers;
using ApiEasier.Dm.Models;
using ApiEasier.Logger.Interfaces;
using System.Text.Json;

namespace ApiEasier.Dal.Repositories.FileStorage
{
    /// <summary>
    /// Обеспечивает работу с API-сервисами через файлы
    /// </summary>
    public class FileApiServiceRepository : IApiServiceRepository
    {
        private readonly IFileHelper _fileHelper;
        private readonly ILoggerService _loggerService;

        public FileApiServiceRepository(IFileHelper fileHelper, ILoggerService loggerService)
        {
            _fileHelper = fileHelper;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Допалняет API-сервис именем, т.к. имя API-сервиса является именем файла конфигурации,
        /// а сам файл не содержит его
        /// </summary>
        /// <param name="name">Название файла без расширения и имя API-сервиса</param>
        /// <param name="apiService">Объект с данными, который нужно дополнить</param>
        /// <returns>Дополненый именем API-сервиса</returns>
        private static ApiService? MapApiService(string name, ApiService? apiService)
        {
            return apiService == null ? null : new ApiService
            {
                Name = name,
                IsActive = apiService.IsActive,
                Description = apiService.Description,
                Entities = apiService.Entities
            };
        }

        public async Task<ApiService?> CreateAsync(ApiService apiService)
        {
            try
            {
                var apiServiceExist = await GetByIdAsync(apiService.Name);
                if (apiServiceExist != null)
                    return default;

                var result = await _fileHelper.WriteAsync(apiService.Name, apiService);
                return result;
            }
            catch
            {
                Console.WriteLine($"Ошибка при записи файла");
                return null;
            }

        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _fileHelper.DeleteAsync(id);
        }

        public async Task<List<ApiService>> GetAllAsync()
        {
            //Именем API-сервиса является название файла его конфигурации
            var apiServiceNames = await _fileHelper.GetAllFileNamesAsync();

            List<ApiService> apiServices = new List<ApiService>();

            foreach (var apiServiceName in apiServiceNames)
            {
                ApiService? apiServiceData;
                try
                {
                    apiServiceData = await _fileHelper.ReadAsync<ApiService>(apiServiceName);
                }
                catch (JsonException ex)
                {
                    _loggerService.LogError(ex, ex.Message);
                    continue;
                }

                if (apiServiceData == null)
                {
                    continue;
                }

                var apiService = MapApiService(apiServiceName, apiServiceData);

                apiServices.Add(apiService!);
            }

            return apiServices;
        }

        public async Task<ApiService?> GetByIdAsync(string id)
        {
            var apiService = await _fileHelper.ReadAsync<ApiService>(id);

            return MapApiService(id, apiService);
        }

        public async Task<ApiService?> UpdateAsync(string id, ApiService apiService)
        {
            try
            {
                var oldApiService = await _fileHelper.ReadAsync<ApiService>(id);

                oldApiService.IsActive = apiService.IsActive;
                oldApiService.Description = apiService.Description;


                if (id != apiService.Name)
                    await _fileHelper.DeleteAsync(id);

                await _fileHelper.WriteAsync(apiService.Name, oldApiService);

                return MapApiService(apiService.Name, apiService);
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
                var apiService = await _fileHelper.ReadAsync<ApiService>(id);

                apiService.IsActive = status;

                await _fileHelper.WriteAsync(id, apiService);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
