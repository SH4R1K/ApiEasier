using ApiEasier.Dal.DB;
using ApiEasier.Dal.Helpers;
using ApiEasier.Dal.Interfaces.Db;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiEasier.Dal.Repositories.Db
{
    public class DbResourceDataRepository : IDbResourceDataRepository
    {
        private readonly MongoDbContext _dbContext;

        public DbResourceDataRepository(MongoDbContext context)
        {
            _dbContext = context;
        }

        public async Task<DynamicResourceModel?> CreateDataAsync(string resourceName, object jsonData)
        {
            // Проверяем, существует ли коллекция
            var collectionExists = await _dbContext.CreateCollectionIfNotExistsAsync(resourceName);

            if (!collectionExists)
            {
                throw new Exception($"Коллекция '{resourceName}' не найдена. Создайте её перед добавлением данных.");
            }

            var collection = _dbContext.GetCollection<BsonDocument>(resourceName);
            var bsonDocument = BsonDocument.Parse(jsonData.ToString());
            await collection.InsertOneAsync(bsonDocument);

            var count = await collection.CountDocumentsAsync(FilterDefinition<BsonDocument>.Empty);
            if (count == 0)
                return default;

            var data = bsonDocument.ToDictionary(
                kvp => kvp.Name,
                kvp => (object)kvp.Value
            );

            return new DynamicResourceModel
            {
                Name = resourceName,
                Data = data
            };
        }

        public Task<DynamicResourceModel> GetDataByIdAsync(string resourceName, string id)
        {
            var collection = _dbContext.GetCollection<BsonDocument>(resourceName);
            return collection;
        }

        public async Task<List<DynamicResourceModel>?> GetAllDataAsync(string resourceName)
        {
            var collection = _dbContext.GetCollection<BsonDocument>(resourceName);

            var documents = await collection.Find(FilterDefinition<BsonDocument>.Empty).ToListAsync();

            var result = documents.Select(doc => new DynamicResourceModel
            {
                Name = doc["_id"].AsObjectId.ToString(), // Если _id это ObjectId, его можно привести к строке
                Data = doc.Elements.ToDictionary(element => element.Name, element => (object)element.Value)
            }).ToList();

            return result ?? null;
        }

        public Task<DynamicResourceModel> UpdateDataAsync(string resourceName, dynamic data)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteDataAsync(string resourceName, string id)
        {
            throw new NotImplementedException();
        }
    }
}
