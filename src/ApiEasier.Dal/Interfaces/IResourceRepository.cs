namespace ApiEasier.Dal.Interfaces
{
    /// <summary>
    /// Позволяет работать с источниками данных для эмуляции
    /// </summary>
    public interface IResourceRepository
    {
        /// <summary>
        /// Удаляет источники данных по имени API-сервиса
        /// </summary>
        /// <param name="id">Идентиффикатор API-сервиса</param>
        /// <returns>Прошло ли удаление</returns>
        public Task<bool> DeleteByApiNameAsync(string id);

        /// <summary>
        /// Обновляет источники данных по имени API-сервиса
        /// </summary>
        /// <param name="id">Текущий идентификатор API-сервиса</param>
        /// <param name="newId">Новый идентификатор API-сервиса</param>
        /// <returns>Прошло ли обновление</returns>
        public Task<bool> UpdateByApiNameAsync(string id, string newId);

        /// <summary>
        /// Удаляет неиспользуемые ресурсы хранения данных, на которых не ссылается ни одна сущность
        /// </summary>
        /// <param name="apiServiceNames">Список всех существующих API-сервисов</param>
        public Task DeleteUnusedResources(List<string> apiServiceNames);
    }
}
