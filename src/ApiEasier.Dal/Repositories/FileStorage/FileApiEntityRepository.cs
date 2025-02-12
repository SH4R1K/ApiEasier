using ApiEasier.Dal.Interfaces.FileStorage;
using ApiEasier.Dal.Interfaces.Helpers;
using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Repositories.FileStorage
{
    public class FileApiEntityRepository : IFileApiEntityRepository
    {
        private readonly IFileHelper _jsonFileHelper;

        public FileApiEntityRepository(IFileHelper jsonFileHelper)
        {
            _jsonFileHelper = jsonFileHelper;
        }

        public async Task<ApiEntity?> CreateAsync(string apiServiceName, ApiEntity apiEntity)
        {
            try
            {
                var apiService = await _jsonFileHelper.ReadAsync<ApiService>(apiServiceName);

                // проверка существования api-сервиса
                if (apiService == null)
                    return null;

                // проверка уникальности
                if (apiService.Entities.Any(e => e.Name == apiEntity.Name))
                    return null ;

                apiService.Entities.Add(apiEntity);

                var result = await _jsonFileHelper.WriteAsync(apiServiceName, apiService);
                if (result == null)
                    return null;

                return apiEntity;
            }
            catch
            {
                Console.WriteLine("Ошибка при добавлении сущности");
                return null;
            }
        }

        public async Task<bool> DeleteAsync(string apiServiceName, string id)
        {
            try
            {
                var apiService = await _jsonFileHelper.ReadAsync<ApiService>(apiServiceName);
                if (apiService == null)
                    return false;

                var entityToRemove = apiService.Entities.FirstOrDefault(e => e.Name == id);
                if (entityToRemove == null)
                    return false;

                apiService.Entities.Remove(entityToRemove);

                await _jsonFileHelper.WriteAsync(apiServiceName, apiService);
                return true;
            }
            catch
            {
                Console.WriteLine("Не удалось удалить сущность");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(string apiServiceName, string id, ApiEntity apiEntity)
        {
            try
            {
                var apiService = await _jsonFileHelper.ReadAsync<ApiService>(apiServiceName);

                if (apiService == null || apiService.Entities == null)
                    return false;

                var entity = apiService.Entities.FirstOrDefault(e => e.Name == id);
                if (entity == null)
                    return false;

                entity.Name = apiEntity.Name;
                entity.Structure = apiEntity.Structure;
                entity.IsActive = apiEntity.IsActive;
                entity.Endpoints = apiEntity.Endpoints;

                await _jsonFileHelper.WriteAsync(apiServiceName, apiService);
                return true;
            }
            catch
            {
                Console.WriteLine("Ошибка при изменении сущности");
                return false;
            }
        }

        public async Task<bool> ChangeActiveStatusAsync(string apiServiceName, string id, bool status)
        {
            try
            {
                var apiService = await _jsonFileHelper.ReadAsync<ApiService>(apiServiceName);
                if (apiService == null || apiService.Entities == null)
                    return false;

                var entity = apiService.Entities.FirstOrDefault(e => e.Name == id);
                if (entity == null)
                    return false;

                entity.IsActive = status;

                await _jsonFileHelper.WriteAsync(id, apiService);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<ApiEntity>> GetAllAsync(string apiServiceName)
        {
            var apiService = await _jsonFileHelper.ReadAsync<ApiService>(apiServiceName);
            if (apiService == null)
                return [];

            return apiService.Entities;
        }

        public async Task<ApiEntity?> GetByIdAsync(string apiServiceName, string id)
        {
            var apiService = await _jsonFileHelper.ReadAsync<ApiService>(apiServiceName);
            if (apiService == null)
                return null;

            return apiService.Entities.FirstOrDefault(e => e.Name == id);
        }
    }
}
