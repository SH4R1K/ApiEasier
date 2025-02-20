using ApiEasier.Dal.Interfaces;
using ApiEasier.Dal.Interfaces.Helpers;
using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Repositories.FileStorage
{
    /// <summary>
    /// Позволяет работать с сущностями через файлы конфигурации
    /// </summary>
    public class FileApiEntityRepository : IApiEntityRepository
    {
        private readonly IFileHelper _fileHelper;

        public FileApiEntityRepository(IFileHelper jsonFileHelper)
        {
            _fileHelper = jsonFileHelper;
        }

        /// <summary>
        /// Считывет файл API-сервиса, добавляет сущность в API-сервис и перезаписывает файл
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// Возникает если API-сервиса не существует, чтобы вернуть ошибку 404 в контроллере
        /// </exception>
        /// <inheritdoc/>
        public async Task<ApiEntity?> CreateAsync(string apiServiceName, ApiEntity apiEntity)
        {
            var apiService = await _fileHelper.ReadAsync<ApiService>(apiServiceName);

            // Проверка существования API-сервиса
            if (apiService == null)
                throw new NullReferenceException($"API-сервис {apiServiceName} не найден");

            // Проверка уникальности
            if (apiService.Entities.Any(e => e.Name == apiEntity.Name))
                return null;

            apiService.Entities.Add(apiEntity);

            await _fileHelper.WriteAsync(apiServiceName, apiService);

            return apiEntity;
        }

        /// <summary>
        /// Считывет файл API-сервиса, удаляет сущность из API-сервиса и перезаписывает файл
        /// </summary>
        /// <param name="id">Имя удаляемой сущности</param>
        /// <exception cref="NullReferenceException">
        /// Возникает если API-сервиса или сущности не существует, чтобы вернуть ошибку 404 в контроллере
        /// </exception>
        /// <inheritdoc/>
        public async Task DeleteAsync(string apiServiceName, string id)
        {
            var apiService = await _fileHelper.ReadAsync<ApiService>(apiServiceName);
            if (apiService == null)
                throw new NullReferenceException($"API-сервис {apiServiceName} не найден");

            var entityToRemove = apiService.Entities.FirstOrDefault(e => e.Name == id);
            if (entityToRemove == null)
                throw new NullReferenceException($"Сущность {id} у API-сервиса {apiServiceName} не была удалена");

            apiService.Entities.Remove(entityToRemove);

            await _fileHelper.WriteAsync(apiServiceName, apiService);
        }

        /// <summary>
        /// Считывает файл с API-сервисом, у найденной по имени сущности меняет свойства,
        /// и перезаписывает файл с новыми данными
        /// </summary>
        /// <param name="id">Имя изменяемой сущности</param>
        /// <exception cref="NullReferenceException">
        /// Возникает если API-сервиса или сущности не существует, чтобы вернуть ошибку 404 в контроллере
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Возникает если новое имя сущности уже существует
        /// </exception>
        /// <inheritdoc/>
        public async Task<ApiEntity> UpdateAsync(string apiServiceName, string id, ApiEntity apiEntity)
        {
            var apiService = await _fileHelper.ReadAsync<ApiService>(apiServiceName);

            if (apiService == null)
                throw new NullReferenceException($"API-сервис {apiServiceName} не найден");

            var entity = apiService.Entities.FirstOrDefault(e => e.Name == id);
            if (entity == null)
                throw new NullReferenceException($"Сущность {id} у API-сервиса {apiServiceName} не была удалена");

            if (id != apiEntity.Name && apiService.Entities.Any(e => e.Name == apiEntity.Name))
                throw new ArgumentException($"Сущность с именем {apiEntity.Name} уже существует.");

            entity.Name = apiEntity.Name;
            entity.Structure = apiEntity.Structure;
            entity.IsActive = apiEntity.IsActive;

            await _fileHelper.WriteAsync(apiServiceName, apiService);
            return entity;
        }

        /// <summary>
        /// Считывает файл с API-сервисом, у найденной по имени сущности меняет свойство isActive,
        /// и перезаписывает файл с новыми данными
        /// </summary>
        /// <param name="id">Имя изменяемой сущности</param>
        /// <inheritdoc/>
        public async Task ChangeActiveStatusAsync(string apiServiceName, string id, bool status)
        {
            var apiService = await _fileHelper.ReadAsync<ApiService>(apiServiceName);
            if (apiService == null)
                throw new NullReferenceException($"API-сервис {apiServiceName} не найден");

            var entity = apiService.Entities.FirstOrDefault(e => e.Name == id);
            if (entity == null)
                throw new NullReferenceException($"Сущность {id} у API-сервиса {apiServiceName} не была удалена");

            entity.IsActive = status;

            await _fileHelper.WriteAsync(id, apiService);
        }

        /// <summary>
        /// Получает из файла API-сервис, а из него уже сущности
        /// </summary>
        /// <inheritdoc/>
        public async Task<List<ApiEntity>?> GetAllAsync(string apiServiceName)
        {
            var apiService = await _fileHelper.ReadAsync<ApiService>(apiServiceName);
            if (apiService == null)
                return null;

            return apiService.Entities;
        }

        /// <summary>
        /// Получает из файла API-сервис, а из него уже требуемую сущность по имени
        /// </summary>
        /// <param name="id">Имя требуемой сущности</param>
        /// <exception cref="NullReferenceException">
        /// Возникает если API-сервиса или сущности не существует, чтобы вернуть ошибку 404 в контроллере
        /// </exception>
        /// <inheritdoc/>
        public async Task<ApiEntity?> GetByIdAsync(string apiServiceName, string id)
        {
            var apiService = await _fileHelper.ReadAsync<ApiService>(apiServiceName);
            if (apiService == null)
                throw new NullReferenceException($"API-сервис {apiServiceName} не найден");

            var entity = apiService.Entities.FirstOrDefault(e => e.Name == id);
            if (entity == null)
                throw new NullReferenceException($"Сущность {id} в API-сервис {apiServiceName} не найден");
            return entity;
        }
    }
}
