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
        public string ConnectionString { get; set; }

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Имя базы данных для логов.
        /// </summary>
        public string LogsDatabaseName { get; set; }

        /// <summary>
        /// Имя коллекции для логов.
        /// </summary>
        public string LogsCollectionName { get; set; }
    }
}
