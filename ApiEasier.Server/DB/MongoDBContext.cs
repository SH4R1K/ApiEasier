using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ApiEasier.Server.DB
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(IOptions<DBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name) =>
            _database.GetCollection<T>(name);

        public async Task<List<string>> ListCollectionNamesAsync()
        {
            var collections = await _database.ListCollectionNamesAsync();
            return await collections.ToListAsync();
        }
    }
}
