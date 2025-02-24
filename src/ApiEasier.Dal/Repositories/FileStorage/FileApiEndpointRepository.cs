using ApiEasier.Dal.Interfaces;
using ApiEasier.Dal.Interfaces.Helpers;
using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Repositories.FileStorage
{
    public class FileApiEndpointRepository : IApiEndpointRepository
    {
        private readonly IFileHelper _fileHelper;

        public FileApiEndpointRepository(IFileHelper jsonFileHelper)
        {
            _fileHelper = jsonFileHelper;
        }

        /// <summary>
        /// Считывает файл с указанным API-сервисом, получает указанную сущность, создает ей эндпоинт 
        /// и записывает новые данные в файл
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// Возникает если API-сервиса, сущности или эндпоинта не существует, чтобы вернуть ошибку 404 в контроллере
        /// </exception>
        /// <inheritdoc/>
        public async Task ChangeActiveStatusAsync(string apiServiceName, string apiEntityName, string id, bool status)
        {
            var apiService = await _fileHelper.ReadAsync<ApiService>(apiServiceName);
            if (apiService == null || apiService.Entities == null)
                throw new NullReferenceException($"API-сервис {apiServiceName} не найден");

            var entity = apiService.Entities.FirstOrDefault(e => e.Name == apiEntityName);
            if (entity == null)
                throw new NullReferenceException($"Сущность {apiEntityName} у API-сервиса {apiServiceName} не была найдена");

            var endpoint = entity.Endpoints.FirstOrDefault(e => e.Route == id);
            if (endpoint == null)
                throw new NullReferenceException($"Эндпоинт {id} у сущности {apiEntityName} API-сервиса {apiServiceName} не была найден");

            endpoint.IsActive = status;

            await _fileHelper.WriteAsync(apiServiceName, apiService);
        }

        /// <summary>
        /// Считывает файл с указанным API-сервисом, получает указанную сущность, создает ей эндпоинт 
        /// и записывает новые данные в файл
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// Возникает если API-сервиса или сущности не существует, чтобы вернуть ошибку 404 в контроллере
        /// </exception>
        /// <inheritdoc/>
        public async Task<ApiEndpoint?> CreateAsync(string apiServiceName, string apiEntityName, ApiEndpoint apiEndpoint)
        {
            var apiService = await _fileHelper.ReadAsync<ApiService>(apiServiceName);
            // Проверка существования API-сервиса
            if (apiService == null)
                throw new NullReferenceException($"API-сервис {apiServiceName} не найден");

            var apiEntity = apiService.Entities.FirstOrDefault(e => e.Name == apiEntityName);
            if (apiEntity == null)
                throw new NullReferenceException($"Сущность {apiEntityName} у API-сервиса {apiServiceName} не была найдена");

            // Проверка уникальности нового имени
            var apiEntityExist = apiEntity.Endpoints.FirstOrDefault(e => e.Route == apiEndpoint.Route);
            if (apiEntityExist != null)
                return null;

            apiEntity.Endpoints.Add(apiEndpoint);

            await _fileHelper.WriteAsync(apiServiceName, apiService);

            return apiEndpoint;
        }

        /// <summary>
        /// Считывает файл с указанным API-сервисом, получает указанную сущность, удаляет ей эндпоинт 
        /// и записывает новые данные в файл
        /// </summary>
        /// <param name="id">Имя требуемого эндпоинта</param>
        /// <exception cref="NullReferenceException">
        /// Возникает если API-сервиса, сущности или эндпоинта не существует, чтобы вернуть ошибку 404 в контроллере
        /// </exception>
        /// <inheritdoc/>
        public async Task DeleteAsync(string apiServiceName, string apiEntityName, string id)
        {
            var apiService = await _fileHelper.ReadAsync<ApiService>(apiServiceName);
            // Проверка существования api-сервиса
            if (apiService == null)
                throw new NullReferenceException($"API-сервис {apiServiceName} не найден");

            var apiEntity = apiService.Entities.FirstOrDefault(e => e.Name == apiEntityName);
            if (apiEntity == null)
                throw new NullReferenceException($"Сущность {apiEntityName} у API-сервиса {apiServiceName} не была найдена");

            var apiEndpointToRemove = apiEntity.Endpoints.FirstOrDefault(e => e.Route == id);
            if (apiEndpointToRemove == null)
                throw new NullReferenceException($"Эндпоинт {id} у сущности {apiEntityName} API-сервиса {apiServiceName} не была найден");

            apiEntity.Endpoints.Remove(apiEndpointToRemove);
        }

        /// <summary>
        /// Считывает файл с указанным API-сервисом, получает указанную сущность и возвращает ее эндпоинты
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// Возникает если API-сервиса или сущности не существует, чтобы вернуть ошибку 404 в контроллере
        /// </exception>
        /// <inheritdoc/>
        public async Task<List<ApiEndpoint>> GetAllAsync(string apiServiceName, string apiEntityName)
        {
            var apiService = await _fileHelper.ReadAsync<ApiService>(apiServiceName);
            if (apiService == null)
                throw new NullReferenceException($"API-сервис {apiServiceName} не найден");

            var apiEntity = apiService.Entities.FirstOrDefault(e => e.Name == apiEntityName);
            if (apiEntity == null)
                throw new NullReferenceException($"Сущность {apiEntityName} у API-сервиса {apiServiceName} не была найдена");

            return apiEntity.Endpoints;
        }

        /// <summary>
        /// Считывает файл с указанным API-сервисом, получает указанную сущность и возвращает требуемый эндпоинт
        /// </summary>
        /// <param name="id">Имя требуемого эндпоинта</param>
        /// <exception cref="NullReferenceException">
        /// Возникает если API-сервиса или сущности не существует, чтобы вернуть ошибку 404 в контроллере
        /// </exception>
        /// <inheritdoc/>
        public async Task<ApiEndpoint?> GetByIdAsync(string apiServiceName, string apiEntityName, string id)
        {
            var apiService = await _fileHelper.ReadAsync<ApiService>(apiServiceName);
            if (apiService == null)
                throw new NullReferenceException($"API-сервис {apiServiceName} не найден");

            var apiEntity = apiService.Entities.FirstOrDefault(e => e.Name == apiEntityName);
            if (apiEntity == null)
                throw new NullReferenceException($"Сущность {apiEntityName} у API-сервиса {apiServiceName} не была найдена");

            return apiEntity.Endpoints.FirstOrDefault(ep => ep.Route == id);
        }

        /// <summary>
        /// Считывает файл с указанным API-сервисом, получает указанную сущность и изменяет требуемый эндпоинт
        /// </summary>
        /// <param name="id">Имя требуемого эндпоинта</param>
        /// <exception cref="NullReferenceException">
        /// Возникает если API-сервиса, сущности или эндпоинта не существует, чтобы вернуть ошибку 404 в контроллере
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Возникает если новое имя эндпоинта уже существует
        /// </exception>
        /// <inheritdoc/>
        public async Task<ApiEndpoint> UpdateAsync(string apiServiceName, string apiEntityName, string id, ApiEndpoint apiEndpoint)
        {
            var apiService = await _fileHelper.ReadAsync<ApiService>(apiServiceName);
            if (apiService == null)
                throw new NullReferenceException($"API-сервис {apiServiceName} не найден");

            var entity = apiService.Entities.FirstOrDefault(e => e.Name == apiEntityName);
            if (entity == null)
                throw new NullReferenceException($"Сущность {apiEntityName} у API-сервиса {apiServiceName} не была найдена");

            var endpoint = entity.Endpoints.FirstOrDefault(e => e.Route == id);
            if (endpoint == null)
                throw new NullReferenceException($"Эндпоинт {id} у сущности {apiEntityName} API-сервиса {apiServiceName} не была найден");

            var endpointExist = entity.Endpoints.FirstOrDefault(e => e.Route == apiEndpoint.Route);
            if (id != apiEndpoint.Route && endpointExist != null)
                throw new ArgumentException($"Имя эндопоинта {apiEndpoint.Route} уже существует");

            endpoint.Route = apiEndpoint.Route;
            endpoint.IsActive = apiEndpoint.IsActive;
            endpoint.Type = apiEndpoint.Type;

            await _fileHelper.WriteAsync(apiServiceName, apiService);

            return endpoint;
        }
    }
}
