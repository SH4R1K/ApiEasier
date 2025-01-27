using ApiEasier.Server.Dto;
using ApiEasier.Server.Models;

namespace ApiEasier.Server.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с конфигурационными файлами API-сервисов.
    /// </summary>
    public interface IConfigFileApiService
    {
        /// <summary>
        /// Асинхронно десериализует API-сервис по его имени.
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса для десериализации.</param>
        /// <returns>Объект <see cref="ApiService"/>, если десериализация успешна; иначе <c>null</c>.</returns>
        Task<ApiService?> DeserializeApiServiceAsync(string apiServiceName);

        /// <summary>
        /// Асинхронно сериализует API-сервис в конфигурационный файл.
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса для сериализации.</param>
        /// <param name="apiService">Объект API-сервиса для сериализации.</param>
        Task SerializeApiServiceAsync(string apiServiceName, ApiService apiService);

        /// <summary>
        /// Асинхронно получает сущность API по имени сущности и имени API-сервиса.
        /// </summary>
        /// <param name="entityName">Имя сущности для получения.</param>
        /// <param name="apiServiceName">Имя API-сервиса, к которому принадлежит сущность.</param>
        /// <returns>Объект <see cref="ApiEntity"/>, если сущность найдена; иначе <c>null</c>.</returns>
        Task<ApiEntity?> GetApiEntityAsync(string entityName, string apiServiceName);

        /// <summary>
        /// Получает имена всех доступных API-сервисов.
        /// </summary>
        /// <returns>Перечисление имен API-сервисов.</returns>
        IEnumerable<string> GetApiServiceNames();

        /// <summary>
        /// Асинхронно получает API-сервис по его имени.
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса для получения.</param>
        /// <returns>Объект <see cref="ApiServiceDto"/>, если сервис найден; иначе <c>null</c>.</returns>
        Task<List<ApiServiceDto>?> GetAllServicesAsync();
        Task<ApiServiceDto?> GetApiServiceByNameAsync(string apiServiceName);

        /// <summary>
        /// Переименовывает существующий API-сервис.
        /// </summary>
        /// <param name="oldName">Старое имя API-сервиса.</param>
        /// <param name="apiServiceDto">Объект DTO с новыми данными API-сервиса.</param>
        void RenameApiService(string oldName, ApiServiceDto apiServiceDto);

        /// <summary>
        /// Удаляет API-сервис по его имени.
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса для удаления.</param>
        void DeleteApiService(string apiServiceName);

        /// <summary>
        /// Проверяет, существует ли API-сервис с заданным именем.
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса для проверки.</param>
        /// <returns><c>true</c>, если сервис существует; иначе <c>false</c>.</returns>
        bool IsApiServiceExist(string apiServiceName);
    }
}
