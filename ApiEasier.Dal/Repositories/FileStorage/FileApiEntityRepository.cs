using ApiEasier.Dal.Helpers;
using ApiEasier.Dal.Interfaces.FileStorage;
using ApiEasier.Dal.Interfaces.Helpers;
using ApiEasier.Dm.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiEasier.Dal.Repositories.FileStorage
{
    public class FileApiEntityRepository : IFileApiEntityRepository
    {
        private readonly IFileHelper _fileHelper;

        public FileApiEntityRepository(IFileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }

        public async Task<bool> CreateAsync(string apiServiceName, ApiEntity apiEntity)
        {
            try
            {
                var apiService = await _fileHelper.ReadJsonAsync<ApiService>(apiServiceName);
                if (apiService == null)
                    return false;

                apiService.Entities.Add(apiEntity);

                await _fileHelper.WriteJsonAsync(apiServiceName, apiService);

                return true;
            }
            catch
            {
                Console.WriteLine("Ошибка при добавлении сущности");
                return false;
            }
            
        }

        public async Task<bool> DeleteAsync(string apiServiceName, string id)
        {
            try
            {
                var apiService = await _fileHelper.ReadJsonAsync<ApiService>(apiServiceName);
                if (apiService == null)
                    return false;

                var entityToRemove = apiService.Entities.FirstOrDefault(e => e.Name == id);
                if (entityToRemove == null)
                    return false;

                apiService.Entities.Remove(entityToRemove);

                await _fileHelper.WriteJsonAsync(apiServiceName, apiService);
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
                var apiService = await _fileHelper.ReadJsonAsync<ApiService>(apiServiceName);

                if (apiService == null || apiService.Entities == null)
                    return false;

                var entity = apiService.Entities.FirstOrDefault(e => e.Name == id);
                if (entity == null)
                    return false;

                entity.Name = apiEntity.Name;
                entity.Structure = apiEntity.Structure;
                entity.IsActive = apiEntity.IsActive;
                entity.Endpoints = apiEntity.Endpoints;

                await _fileHelper.WriteJsonAsync(apiServiceName, apiService);
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
                var apiService = await _fileHelper.ReadJsonAsync<ApiService>(id);
                if (apiService == null || apiService.Entities == null)
                    return false;

                var entity = apiService.Entities.FirstOrDefault(e => e.Name == id);
                if (entity == null)
                    return false;

                entity.IsActive = status;

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
