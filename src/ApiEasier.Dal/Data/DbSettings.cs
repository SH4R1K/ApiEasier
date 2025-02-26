namespace ApiEasier.Dal.Data
{
    /// <summary>
    /// Настройки базы данных.
    /// </summary>
    public class DbSettings
    {
        /// <summary>
        /// Строка подключения к базе данных.
        /// </summary>
        public required string ConnectionString { get; set; }

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        public required string DatabaseName { get; set; }

        /// <summary>
        /// Имя базы данных для логов.
        /// </summary>
        public required string LogsDatabaseName { get; set; }

        /// <summary>
        /// Имя коллекции для логов.
        /// </summary>
        public required string LogsCollectionName { get; set; }
    }
}
