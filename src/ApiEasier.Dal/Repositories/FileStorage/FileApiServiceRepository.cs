﻿using ApiEasier.Dal.Interfaces;
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

        /// <summary>
        /// Записывает новый API-сервис в файл
        /// </summary>
        /// <inheritdoc/>
        public async Task<ApiService?> CreateAsync(ApiService apiService)
        {
            var apiServiceExist = await GetByIdAsync(apiService.Name);
            if (apiServiceExist != null)
                return null;

            var result = await _fileHelper.WriteAsync(apiService.Name, apiService);
            return result;
        }

        public async Task DeleteAsync(string id)
        {
            await _fileHelper.DeleteAsync(id);
        }

        /// <summary>
        /// Считывает папку с файлами конфигурации и считывает их все данные
        /// </summary>
        /// <inheritdoc/>
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

        /// <summary>
        /// Считывает файл с конфигурацией API-сервиса
        /// </summary>
        /// <param name="id">Имя файла без расширения</param>
        /// <inheritdoc/>
        public async Task<ApiService?> GetByIdAsync(string id)
        {
            var apiService = await _fileHelper.ReadAsync<ApiService>(id);

            return MapApiService(id, apiService);
        }

        /// <summary>
        /// Считывает файл и заменяет данные на новые, а если изменено имя, удаляет старый файл
        /// и создает новый с изменеными данными из старого файла
        /// </summary>
        /// <param name="id">Имя файла без расширения</param>
        /// <exception cref="NullReferenceException">
        /// Возникает если изменяемого API-сервиса не существует, чтобы вернуть ошибку 404 в контроллере
        /// </exception>
        /// <inheritdoc/>
        public async Task<ApiService?> UpdateAsync(string id, ApiService apiService)
        {
            var oldApiService = await _fileHelper.ReadAsync<ApiService>(id);

            if (oldApiService == null)
                throw new NullReferenceException();

            oldApiService.IsActive = apiService.IsActive;
            oldApiService.Description = apiService.Description;

            var apiServiceExist = await GetByIdAsync(apiService.Name);
            if (apiServiceExist != null)
                return null;

            if (id != apiService.Name)
                await _fileHelper.DeleteAsync(id);

            await _fileHelper.WriteAsync(apiService.Name, oldApiService);

            return MapApiService(apiService.Name, apiService);
        }

        /// <summary>
        /// Считывает файл с API-сервисом и изменяет значение isActive и записывает обратно
        /// </summary>
        /// <inheritdoc/>
        public async Task<bool> ChangeActiveStatusAsync(string id, bool status)
        {
            var apiService = await _fileHelper.ReadAsync<ApiService>(id);

            if (apiService == null)
                return false;

            apiService.IsActive = status;

            await _fileHelper.WriteAsync(id, apiService);
            return true;
        }
    }
}
