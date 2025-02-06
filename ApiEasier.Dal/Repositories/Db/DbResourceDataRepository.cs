using ApiEasier.Dal.Data;
using ApiEasier.Dal.Interfaces.Db;
using ApiEasier.Dm.Models.Dynamic;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json.Nodes;

namespace ApiEasier.Dal.Repositories.Db
{
    public class DbResourceDataRepository : IDbResourceDataRepository
    {
        private readonly MongoDbContext _dbContext;

        public DbResourceDataRepository(MongoDbContext context)
        {
            _dbContext = context;
        }

        public async Task<DynamicResource> CreateDataAsync(string resourceName, object jsonData)
        {
            var collection = _dbContext.GetCollection<BsonDocument>(resourceName);
            var bsonDocument = BsonDocument.Parse(jsonData.ToString());
            await collection.InsertOneAsync(bsonDocument);

            return new DynamicResource
            {
                Data = JsonNode.Parse(bsonDocument.ToJson())
            };
        }

        public async Task<DynamicResource> GetDataByIdAsync(string resourceName, string id)
        {
            var collection = _dbContext.GetCollection<BsonDocument>(resourceName);

            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var bsonDocument = await collection.Find(filter).FirstOrDefaultAsync();

            return new DynamicResource
            {
                Data = JsonNode.Parse(bsonDocument.ToJson())
            };
        }

        public async Task<List<DynamicResource>?> GetAllDataAsync(string resourceName)
        {
            var collection = _dbContext.GetCollection<BsonDocument>(resourceName);

            var documents = await collection.Find(FilterDefinition<BsonDocument>.Empty).ToListAsync();

            return documents.Select(doc => new DynamicResource
            {
                Data = JsonNode.Parse(doc.ToJson())
            }).ToList();
        }

        public async Task<bool> DeleteDataAsync(string resourceName, string id)
        {
            var collection = _dbContext.GetCollection<BsonDocument>(resourceName);

            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

            var result = await collection.DeleteOneAsync(filter);

            if (result.DeletedCount == 0)
                return false;

            return true;
        }

        public async Task<DynamicResource> UpdateDataAsync(string resourceName, string id, object data)
        {
            var collection = _dbContext.GetCollection<BsonDocument>(resourceName);

            var objectId = new ObjectId(id);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);
            var update = Builders<BsonDocument>.Update.Set("data", data.ToBsonDocument());

            var result = await collection.UpdateOneAsync(filter, update);

            if (result.ModifiedCount == 0)
                return null;

            var updatedDocument = await collection.Find(filter).FirstOrDefaultAsync();

            return new DynamicResource
            {
                Data = JsonNode.Parse(updatedDocument.ToJson())
            };
        }
    }
}
