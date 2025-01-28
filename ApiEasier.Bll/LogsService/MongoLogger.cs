using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiEasier.Bll.LogsService
{
    /// <summary>
    /// Класс для записи логов в конкретную коллекцию в MongoDB
    /// </summary>
    public class MongoLogger : ILogger
    {
        private readonly IMongoCollection<BsonDocument> _logCollection;
        private readonly string _categoryName;

        public MongoLogger(IMongoCollection<BsonDocument> logCollection, string categoryName)
        {
            _logCollection = logCollection;
            _categoryName = categoryName;
        }

        public IDisposable? BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        /// <summary>
        /// Главный метод записи лога
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel">Уровень логирования (например, Information, Error, и т. д.).</param>
        /// <param name="eventId">Уникальный идентификатор события</param>
        /// <param name="state">Данные состояния лога</param>
        /// <param name="exception">Исключение, которое может быть записано в лог.</param>
        /// <param name="formatter">Функция, которая используется для форматирования лог-сообщения.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);

            // Попробуем найти "Request and Response" и разобрать его.
            if (message.Contains("Request and Response:"))
            {
                var parts = message.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                var logEntry = new BsonDocument
                {
                    { "Category", _categoryName },
                    { "LogLevel", logLevel.ToString() },
                    { "EventId", eventId.Id },
                    { "Timestamp", DateTime.UtcNow }
                };

                // Разбираем данные
                foreach (var part in parts)
                {
                    var splitIndex = part.IndexOf(": ");
                    if (splitIndex > 0)
                    {
                        var key = part.Substring(0, splitIndex).Trim();
                        var value = part.Substring(splitIndex + 2).Trim();
                        logEntry[key] = value;
                    }
                }

                // запись логов после того как данные собраны в BsonDocument
                _logCollection.InsertOne(logEntry);
            }
        }
    }
}
