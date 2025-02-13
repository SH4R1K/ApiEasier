using ApiEasier.Dal.Data;
using ApiEasier.Dal.Interfaces;
using ApiEasier.Dm.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json.Nodes;

namespace ApiEasier.Dal.Repositories.Db
{
    public class DbResourceDataRepository : IResourceDataRepository
    {
        private readonly MongoDbContext _dbContext;

        public DbResourceDataRepository(MongoDbContext context)
        {
            _dbContext = context;
        }

        public async Task<DynamicResourceData> CreateDataAsync(string resourceName, object jsonData)
        {
            var collection = _dbContext.GetCollection<BsonDocument>(resourceName);
            var bsonDocument = BsonDocument.Parse(jsonData.ToString());
            await collection.InsertOneAsync(bsonDocument);

            var bsonDoc = bsonDocument.ToDictionary();
            bsonDoc["_id"] = bsonDocument["_id"].ToString();

            return new DynamicResourceData
            {
                Data = JsonNode.Parse(bsonDoc.ToJson())
            };
        }

        public async Task<DynamicResourceData?> GetDataByIdAsync(string resourceName, string id)
        {
            var collection = _dbContext.GetCollection<BsonDocument>(resourceName);

            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);
            var bsonDocument = await collection.Find(filter).FirstOrDefaultAsync();

            var bsonDoc = bsonDocument.ToDictionary();
            bsonDoc["_id"] = bsonDocument["_id"].ToString();

            return new DynamicResourceData
            {
                Data = JsonNode.Parse(bsonDoc.ToJson())
            };
        }

        public async Task<List<DynamicResourceData>?> GetAllDataAsync(string resourceName)
        {
            var collection = _dbContext.GetCollection<BsonDocument>(resourceName);

            var documents = await collection.Find(FilterDefinition<BsonDocument>.Empty).ToListAsync();

            return documents.Select(doc =>
            {
                var bsonDoc = doc.ToDictionary();
                bsonDoc["_id"] = doc["_id"].ToString();

                return new DynamicResourceData
                {
                    Data = JsonNode.Parse(bsonDoc.ToJson())
                };

            }).ToList();
        }

        public async Task<bool> DeleteDataAsync(string resourceName, string id)
        {
            var collection = _dbContext.GetCollection<BsonDocument>(resourceName);

            if (!ObjectId.TryParse(id, out var objectId))
                return false;

            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);

            var result = await collection.DeleteOneAsync(filter);

            if (result.DeletedCount == 0)
                return false;

            return true;
        }

        public async Task<DynamicResourceData?> UpdateDataAsync(string resourceName, string id, object data)
        {
            var collection = _dbContext.GetCollection<BsonDocument>(resourceName);

            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);

            var dataStr = data.ToString();
            var newDocument = BsonDocument.Parse(dataStr);

            // Сохраняем _id в новом документе, чтобы оно не изменилось
            newDocument["_id"] = objectId;

            var result = await collection.ReplaceOneAsync(filter, newDocument);

            if (result.ModifiedCount == 0)
                return null;

            var updatedDocument = await collection.Find(filter).FirstOrDefaultAsync();

            var bsonDoc = updatedDocument.ToDictionary();
            bsonDoc["_id"] = updatedDocument["_id"].ToString();

            return new DynamicResourceData
            {
                Data = JsonNode.Parse(bsonDoc.ToJson())
            };
        }
    }
}
