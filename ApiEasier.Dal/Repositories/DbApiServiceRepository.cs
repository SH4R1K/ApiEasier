using ApiEasier.Dal.DB;
using ApiEasier.Dal.Helpers;
using ApiEasier.Dal.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiEasier.Dal.Repositories
{
    public class DbApiServiceRepository : IDbApiServiceRepository
    {
        private readonly MongoDbContext _dbContext;

        public DbApiServiceRepository(MongoDbContext context)
        {
            _dbContext = context;
        }

        public async Task<DynamicCollectionModel?> CreateAsync(dynamic apiServiceName, object jsonData)
        {
            var collection = await _dbContext.GetCollection<BsonDocument>(apiServiceName);
            var bsonDocument = BsonDocument.Parse(jsonData.ToString());
            await collection.InsertOneAsync(bsonDocument);

            if (collection.Count == 0)
                return default;

            var data = bsonDocument.ToDictionary(
                kvp => kvp.Name,
                kvp => (object)kvp.Value
            );

            var result = new DynamicCollectionModel
            {
                Name = apiServiceName,
                Data = data
            };

            return result;
        }

        public async Task<bool> DeleteAsync(string apiServiceName)
        {
            var result = await _dbContext.DropCollectionAsync(apiServiceName);
            return result;
        }

        public Task<DynamicCollectionModel> GetDataByIdAsync(dynamic apiServiceName, string id)
        {
            var collection = _dbContext.GetCollection<BsonDocument>(apiServiceName);
        }

        public async Task<List<DynamicCollectionModel>?> GetDataAsync(string apiServiceName)
        {
            var collection = _dbContext.GetCollection<BsonDocument>(apiServiceName);

            var documents = await collection.Find(FilterDefinition<BsonDocument>.Empty).ToListAsync();

            var result = documents.Select(doc => new DynamicCollectionModel
            {
                Name = doc["_id"].AsObjectId.ToString(), // Если _id это ObjectId, его можно привести к строке
                Data = doc.Elements.ToDictionary(element => element.Name, element => (object)element.Value)
            }).ToList();

            return result ?? null;
        }

        public Task<DynamicCollectionModel> UpdateAsync(dynamic apiServiceName, dynamic data)
        {
            throw new NotImplementedException();
        }
    }
}
