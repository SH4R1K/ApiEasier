using ApiEasier.Server.DB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiEasier.Server.LogsService
{
    public class MongoLoggerProvider : ILoggerProvider
    {
        private readonly IMongoCollection<BsonDocument> _logCollection;
        private readonly MongoDBContext _dbContext;

        public MongoLoggerProvider(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _logCollection = database.GetCollection<BsonDocument>(collectionName);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new MongoLogger(_logCollection, categoryName);
        }

        public void Dispose() { }
    }
}
