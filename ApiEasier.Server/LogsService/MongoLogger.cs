
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiEasier.Server.LogsService
{
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

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);

            // Попробуем найти "Request and Response" и разобрать его.
            if (message.Contains("Request and Response:"))
            {
                var parts = message.Split(new[] { "\r\n" }, StringSplitOptions.None);

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

                _logCollection.InsertOne(logEntry);
            }
        }
    }
}
