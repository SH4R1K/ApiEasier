using ApiEasier.Dal.Interfaces.FileStorage;
using ApiEasier.Dal.Interfaces.Helpers;
using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Repositories.FileStorage
{
    public class FileApiServiceRepository : IFileApiServiceRepository
    {
        private readonly IFileHelper _jsonFileHelper;

        public FileApiServiceRepository(IFileHelper jsonFileHelper)
        {
            _jsonFileHelper = jsonFileHelper;
        }

        /// <summary>
        /// Требуется метод маппинга который будет заполнять поле имени у apiService
        /// т.к в json-файле конфигурации имя api-сервсиа не хранится,
        /// оно содержится в самом названии файла
        /// </summary>
        /// <param name="name">название файла</param>
        /// <param name="apiService">объект с данными для apiService</param>
        /// <returns></returns>
        private ApiService MapApiService(string name, ApiService apiService)
        {
            return new ApiService(name)
            {
                IsActive = apiService.IsActive,
                Description = apiService.Description,
                Entities = apiService.Entities
            };
        }
        
        public async Task<ApiService?> CreateAsync(ApiService apiService)
        {
            try
            {
                var apiServiceExist = GetByIdAsync(apiService.Name);
                if (apiServiceExist != null)
                    return default;

                var result = await _jsonFileHelper.WriteAsync(apiService.Name, apiService);
                return result;
            }
            catch
            {
                Console.WriteLine($"Ошибка при записи файла");
                return null;
            }

        }

        public bool Delete(string id)
        {
            try
            {
                var filePath = _jsonFileHelper.Delete(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<ApiService>> GetAllAsync()
        {
            var filesNames = await _jsonFileHelper.GetAllFilesAsync();

            List<ApiService> apiServices = new List<ApiService>();
            
            foreach (var fileName in filesNames)
            {
                var apiServiceData = await _jsonFileHelper.ReadAsync<ApiService>(fileName); 

                if (apiServiceData == null)
                    continue;

                var apiService = MapApiService(fileName, apiServiceData);

                if (apiService != null)
                    apiServices.Add(apiService);
            }

            return apiServices;
        }

        public async Task<ApiService?> GetByIdAsync(string id)
        {
            var apiService = await _jsonFileHelper.ReadAsync<ApiService>(id);

            return apiService == null ? null : MapApiService(id, apiService);
        }

        public async Task<ApiService?> UpdateAsync(string id, ApiService apiService)
        {
            try
            {
                var oldApiService = await _jsonFileHelper.ReadAsync<ApiService>(id);

                oldApiService.IsActive = apiService.IsActive;
                oldApiService.Description = apiService.Description;
                oldApiService.Entities = apiService.Entities;


                if (id != apiService.Name)
                    _jsonFileHelper.Delete(id);

                await _jsonFileHelper.WriteAsync(apiService.Name, oldApiService);

                return oldApiService == null ? null : MapApiService(apiService.Name, apiService);
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
                var apiService = await _jsonFileHelper.ReadAsync<ApiService>(id);

                apiService.IsActive = status;

                await _jsonFileHelper.WriteAsync(id, apiService);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
