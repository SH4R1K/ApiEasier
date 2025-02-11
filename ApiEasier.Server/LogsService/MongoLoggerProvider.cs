using ApiEasier.Server.Db;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiEasier.Server.LogsService
{
    /// <summary>
    /// Этот класс является провайдером логгера, который создаёт экземпляр MongoLogger для использования в приложении.
    /// Провайдер инициализирует MongoDB клиент, получает базу данных и коллекцию для логирования. Затем он передает их в MongoLogger.
    /// </summary>
    public class MongoLoggerProvider : ILoggerProvider
    {
        private readonly IMongoCollection<BsonDocument> _logCollection;

        public MongoLoggerProvider(IOptions<DbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.LogsDatabaseName);
            _logCollection = database.GetCollection<BsonDocument>(settings.Value.LogsCollectionName);
        }

        /// <summary>
        /// Создание экземпляра логгера
        /// </summary>
        /// <param name="categoryName">Название коллекции в MongoDB куда будут идти логи в виде документов</param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new MongoLogger(_logCollection, categoryName);
        }

        public void Dispose() { }
    }
}
