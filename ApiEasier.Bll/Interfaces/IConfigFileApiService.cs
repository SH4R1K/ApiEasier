using ApiEasier.Bll.Dto;
using ApiEasier.Server.Models;

namespace ApiEasier.Bll.Interfaces
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
        /// Асинхронно получает API-сервис по его имени.
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса для получения.</param>
        /// <returns>Объект <see cref="ApiServiceDto"/>, если сервис найден; иначе <c>null</c>.</returns>
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

        /// <summary>
        /// Получает данныне API-сервисов в директории.
        /// </summary>
        /// <param name="searchTerm">Термин для поиска по именам API-сервисов (необязательно).</param>
        /// <param name="page">Номер страницы для пагинации (необязательно).</param>
        /// <param name="pageSize">Количество элементов на странице (необязательно, по умолчанию 10).</param>
        /// <returns>Список имен API-сервисов.</returns>
        Task<List<ApiServiceDto>> GetApiServicesAsync(int? page = null, string? searchTerm = null, int? pageSize = 10);
    }
}
