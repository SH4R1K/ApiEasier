namespace ApiEasier.Server.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с динамическими коллекциями в MongoDB.
    /// </summary>
    public interface IDynamicCollectionService
    {
        /// <summary>
        /// Асинхронно добавляет документ в указанную коллекцию.
        /// </summary>
        /// <param name="collectionName">Имя коллекции, в которую будет добавлен документ.</param>
        /// <param name="jsonData">Данные документа в формате JSON.</param>
        /// <returns>
        /// Словарь, представляющий добавленный документ, или <c>null</c>, если добавление не удалось.
        /// </returns>
        Task<Dictionary<string, object>?> AddDocToCollectionAsync(string collectionName, object jsonData);

        /// <summary>
        /// Асинхронно получает документы из указанной коллекции с применением фильтров.
        /// </summary>
        /// <param name="collectionName">Имя коллекции, из которой будут получены документы.</param>
        /// <param name="filters">Фильтры для получения документов, или <c>null</c> для получения всех документов.</param>
        /// <returns>
        /// Список словарей, представляющих документы, или <c>null</c>, если не найдено.
        /// </returns>
        Task<List<Dictionary<string, object>?>> GetDocFromCollectionAsync(string collectionName, string? filters);

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
    }
}
