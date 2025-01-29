using ApiEasier.Dal.Helpers;

namespace ApiEasier.Bll.Interfaces.ApiEmu
{
    /// <summary>
    /// Интерфейс для работы с динамическими коллекциями в MongoDB.
    /// </summary>
    public interface IDynamicApiService
    {

        Task<DynamicApiServiceModel> AddDataAsync(string collectionName, object jsonData);

        Task<List<DynamicApiServiceModel>> GetDataAsync(string collectionName, string? filters);

        /// <summary>
        /// Асинхронно получает документ из указанной коллекции по его ID.
        /// </summary>
        /// <param name="collectionName">Имя коллекции, из которой будет получен документ.</param>
        /// <param name="id">ID документа для получения.</param>
        /// <param name="filters">Дополнительные фильтры для получения документа, или <c>null</c>.</param>
        /// <returns>
        /// Словарь, представляющий документ, или <c>null</c>, если документ не найден.
        /// </returns>
        Task<Dictionary<string, object>?> GetDocByIdFromCollectionAsync(string collectionName, string id, string? filters);

        /// <summary>
        /// Асинхронно обновляет документ в указанной коллекции по его ID.
        /// </summary>
        /// <param name="collectionName">Имя коллекции, в которой будет обновлен документ.</param>
        /// <param name="id">ID документа для обновления.</param>
        /// <param name="jsonData">Данные для обновления документа в формате JSON.</param>
        /// <returns>
        /// Словарь, представляющий обновленный документ, или <c>null</c>, если документ не найден.
        /// </returns>
        Task<Dictionary<string, object>?> UpdateDocFromCollectionAsync(string collectionName, string id, object jsonData);

        /// <summary>
        /// Асинхронно удаляет документ из указанной коллекции по его ID.
        /// </summary>
        /// <param name="collectionName">Имя коллекции, из которой будет удален документ.</param>
        /// <param name="id">ID документа для удаления.</param>
        /// <returns>
        /// Количество удаленных документов, или <c>null</c>, если коллекция не найдена.
        /// </returns>
        Task<long?> DeleteDocFromCollectionAsync(string collectionName, string id);

        /// <summary>
        /// Удаляет коллекции по удалении конфигурации API-сервиса.
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса</param>
        Task DeleteCollectionsByDeletedApiServiceAsync(string apiServiceName);

        /// <summary>
        /// Удаляет коллекцию по изменении конфигурации API-сервиса.
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса</param>
        Task DeleteCollectionsByChangedApiServiceAsync(string apiServiceName);

        /// <summary>
        /// Переименовывает коллекцию по новому имени API-сервиса.
        /// </summary>
        /// <param name="oldApiServiceName">Старое имя API-сервиса</param>
        /// <param name="apiServiceName">Новое имя API-сервиса</param>
        Task RenameCollectionsByApiServiceAsync(string oldApiServiceName, string apiServiceName);

        /// <summary>
        /// Находит неиспользуемые коллекции по всем файлам конфигурации и удаляет их
        /// </summary>
        Task DeleteTrashCollectionAsync();
    }
}
