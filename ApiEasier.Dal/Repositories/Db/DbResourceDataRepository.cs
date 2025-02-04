using ApiEasier.Dal.Data;
using ApiEasier.Dal.Interfaces.Db;
using ApiEasier.Dm.Models.Dynamic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiEasier.Dal.Repositories.Db
{

    //TODO Возможно переделать на другой возврат
    public class DbResourceDataRepository : IDbResourceDataRepository
    {
        private readonly MongoDbContext _dbContext;

        public DbResourceDataRepository(MongoDbContext context)
        {
            _dbContext = context;
        }

        public async Task<DynamicResource?> CreateDataAsync(string resourceName, object jsonData)
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

            return new DynamicResource
            {
                Name = resourceName,
                Data = data
            };
        }

        public async Task<DynamicResourceData> GetDataByIdAsync(string resourceName, string id)
        {
            var collection = _dbContext.GetCollection<BsonDocument>(resourceName);

            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var document = await collection.Find(filter).FirstOrDefaultAsync();

            var result = document.ToDictionary(
                kvp => kvp.Name,
                kvp => (object)kvp.Value
            );

            return new DynamicResourceData
            {
                Data = result
            };
        }

        public async Task<List<DynamicResource>?> GetAllDataAsync(string resourceName)
        {
            var collection = _dbContext.GetCollection<BsonDocument>(resourceName);

            var documents = await collection.Find(FilterDefinition<BsonDocument>.Empty).ToListAsync();

            var result = documents.Select(doc => new DynamicResource
            {
                Name = doc["_id"].AsObjectId.ToString(), // Если _id это ObjectId, его можно привести к строке
                Data = doc.Elements.ToDictionary(element => element.Name, element => (object)element.Value)
            }).ToList();

            return result ?? null;
        }

        public async Task<bool> DeleteDataAsync(string resourceName, string id)
        {
            var collection = _dbContext.GetCollection<BsonDocument>(resourceName);

            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

            var result = await collection.DeleteOneAsync(filter);

            if (result.DeletedCount > 0)
                return true;

            return false;

        }

        public async Task<DynamicResource> UpdateDataAsync(string resourceName, string id, object data)
        {
            var collection = _dbContext.GetCollection<BsonDocument>(resourceName);

            var objectId = new ObjectId(id);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);
            var update = Builders<BsonDocument>.Update.Set("data", data.ToBsonDocument());

            var result = await collection.UpdateOneAsync(filter, update);

            if (result.ModifiedCount > 0)
            {
                var updatedDocument = await collection.Find(filter).FirstOrDefaultAsync();
                return new DynamicResource
                {
                    Name = id,
                    Data = updatedDocument.ToDictionary()
                };
            }

            return null;
        }
    }
}
