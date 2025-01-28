using MongoDB.Driver;

namespace ApiEasier.Server.Db
{
    /// <summary>
    /// Контекст для работы с MongoDB.
    /// </summary>
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name) =>
            _database.GetCollection<T>(name);

        public async Task<List<string>> GetListCollectionNamesAsync()
        {
            var collections = await _database.ListCollectionNamesAsync();
            return await collections.ToListAsync();
        }

        public async Task DropCollectionAsync(string name)
        {
            await _database.DropCollectionAsync(name);
        }

        public async Task RenameCollectionAsync(string oldName, string newName)
        {
            await _database.RenameCollectionAsync(oldName, newName);
        }
    }
}
