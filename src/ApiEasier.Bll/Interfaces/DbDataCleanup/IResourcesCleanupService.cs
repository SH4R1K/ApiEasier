namespace ApiEasier.Bll.Interfaces.DbDataCleanup
{
    /// <summary>
    /// Позволяет очищать неиспользуемые ресурсы хранения данных, на которых не ссылается ни одна сущность
    /// </summary>
    public interface IResourcesCleanupService
    {
        /// <summary>
        /// Удаляет неиспользуемые ресурсы хранения данных, на которых не ссылается ни одна сущность
        /// </summary>
        public Task CleanupAsync();
    }
}
