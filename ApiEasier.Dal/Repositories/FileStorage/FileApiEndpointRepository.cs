﻿using ApiEasier.Dal.Interfaces.FileStorage;
using ApiEasier.Dal.Interfaces.Helpers;
using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Repositories.FileStorage
{
    public class FileApiEndpointRepository : IFileApiEndpointRepository
    {
        private readonly IFileHelper _jsonFileHelper;

        public FileApiEndpointRepository(IFileHelper jsonFileHelper)
        {
            _jsonFileHelper = jsonFileHelper;
        }

        public async Task<bool> ChangeActiveStatusAsync(string apiServiceName, string apiEntityName, string id, bool status)
        {
            try
            {
                var apiService = await _jsonFileHelper.ReadAsync<ApiService>(id);
                if (apiService == null || apiService.Entities == null)
                    return false;

                var entity = apiService.Entities.FirstOrDefault(e => e.Name == id);
                if (entity == null)
                    return false;

                var endpoint = entity.Endpoints.FirstOrDefault(e => e.Route == id);
                if (endpoint == null)
                    return false;

                endpoint.IsActive = status;

                await _jsonFileHelper.WriteAsync(apiServiceName, apiService);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CreateAsync(string apiServiceName, string apiEntityName, ApiEndpoint apiEndpoint)
        {
            try
            {
                var apiService = await _jsonFileHelper.ReadAsync<ApiService>(apiServiceName);
                // проверка существования api-сервиса
                if (apiService == null)
                    return false;

                var apiEntity = apiService.Entities.FirstOrDefault(e => e.Name == apiEntityName);
                if (apiEntity == null)
                    return false;

                apiEntity.Endpoints.Add(apiEndpoint);

                await _jsonFileHelper.WriteAsync(apiServiceName, apiService);

                return true;
            }
            catch
            {
                Console.WriteLine("Ошибка при добавлении сущности");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string apiServiceName, string apiEntityName, string id)
        {
            try
            {
                var apiService = await _jsonFileHelper.ReadAsync<ApiService>(apiServiceName);
                // проверка существования api-сервиса
                if (apiService == null)
                    return false;

                var apiEntity = apiService.Entities.FirstOrDefault(e => e.Name == apiEntityName);
                if (apiEntity == null)
                    return false;

                var apiEndpointToRemove = apiEntity.Endpoints.FirstOrDefault(e => e.Route == id);
                if (apiEndpointToRemove == null)
                    return false;

                apiEntity.Endpoints.Remove(apiEndpointToRemove);
                return true;
            }
            catch
            {
                Console.WriteLine("Не удалось удалить сущность");
                return false;
            }
        }

        public async Task<List<ApiEndpoint>> GetAllAsync(string apiServiceName, string apiEntityName)
        {
            var apiService = await _jsonFileHelper.ReadAsync<ApiService>(apiServiceName);
            if (apiService == null)
                return [];

            var apiEntity = apiService.Entities.FirstOrDefault(e => e.Name == apiEntityName);
            if (apiEntity == null)
                return [];

            return apiEntity.Endpoints;
        }

        public async Task<ApiEndpoint?> GetByIdAsync(string apiServiceName, string apiEntityName, string id)
        {
            var apiService = await _jsonFileHelper.ReadAsync<ApiService>(apiServiceName);
            if (apiService == null)
                return null;

            var apiEntity = apiService.Entities.FirstOrDefault(e => e.Name == apiEntityName);
            if (apiEntity == null)
                return null;


            return apiEntity.Endpoints.FirstOrDefault(ep => ep.Route == id);
        }

        public async Task<bool> UpdateAsync(string apiServiceName, string apiEntityName, string id, ApiEndpoint apiEndpoint)
        {
            try
            {
                var apiService = await _jsonFileHelper.ReadAsync<ApiService>(id);
                if (apiService == null || apiService.Entities == null)
                    return false;

                var entity = apiService.Entities.FirstOrDefault(e => e.Name == id);
                if (entity == null)
                    return false;

                var endpoint = entity.Endpoints.FirstOrDefault(e => e.Route == id);
                if (endpoint == null)
                    return false;

                endpoint.Route = apiEndpoint.Route;
                endpoint.IsActive = apiEndpoint.IsActive;
                endpoint.Type = apiEndpoint.Type;

                await _jsonFileHelper.WriteAsync(apiServiceName, apiService);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
