using ApiEasier.Dal.Data;
using ApiEasier.Dal.Interfaces;
using ApiEasier.Dm.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Text.Json.Nodes;

namespace ApiEasier.Dal.Repositories.Db
{
    /// <summary>
    /// Позволяет управлять данными сущности через MongoDB
    /// </summary>
    public class DbResourceDataRepository : IResourceDataRepository
    {
        private readonly MongoDbContext _dbContext;

        public DbResourceDataRepository(MongoDbContext context)
        {
            _dbContext = context;
        }

        /// <summary>
        /// Добавляет новый документ в коллекцию по имени ресурса
        /// </summary>
        /// <inheritdoc/>
        public async Task<DynamicResourceData> CreateDataAsync(string resourceName, object data)
        {
            var collection = _dbContext.GetCollection<BsonDocument>(resourceName);
            var bsonDocument = BsonDocument.Parse(data.ToString());
            await collection.InsertOneAsync(bsonDocument);

            var bsonDoc = bsonDocument.ToDictionary();
            bsonDoc["_id"] = bsonDocument["_id"].ToString();

            return new DynamicResourceData
            {
                Data = JsonNode.Parse(bsonDoc.ToJson())!
            };
        }

        /// <summary>
        /// Возвращает данные объекта по идентификатору из коллекции с названием имени ресурса
        /// </summary>
        /// <inheritdoc/>
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
                Data = JsonNode.Parse(bsonDoc.ToJson())!
            };
        }

        /// <summary>
        /// Возвращает все или отфильтрованные данные из коллекции по имени ресурса
        /// </summary>
        /// <inheritdoc/>
        public async Task<List<DynamicResourceData>?> GetAllDataAsync(string resourceName, string? filter)
        {
            var collection = _dbContext.GetCollection<BsonDocument>(resourceName);

            var filterDb = FilterDefinition<BsonDocument>.Empty;

            if (filter != null)
                filterDb = BsonSerializer.Deserialize<BsonDocument>(filter);

            var documents = await collection.Find(filterDb).ToListAsync();

            return documents.Select(doc =>
            {
                var bsonDoc = doc.ToDictionary();
                bsonDoc["_id"] = doc["_id"].ToString();

                return new DynamicResourceData
                {
                    Data = JsonNode.Parse(bsonDoc.ToJson())!
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

        /// <summary>
        /// Изменяет документ в коллекции по идентификатору
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Возникает, если идентфикатор имеет неверный формат
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        /// Возникает, если такой объект был не найден для изменения
        /// </exception>
        /// <inheritdoc/>
        public async Task<DynamicResourceData?> UpdateDataAsync(string resourceName, string id, object data)
        {
            var collection = _dbContext.GetCollection<BsonDocument>(resourceName);

            if (!ObjectId.TryParse(id, out var objectId))
                throw new ArgumentException("Неверный формат идентификатора");

            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);

            var dataStr = data.ToString();
            var newDocument = BsonDocument.Parse(dataStr);
            newDocument["_id"] = objectId; // Сохраняем исходный _id

            var result = await collection.ReplaceOneAsync(filter, newDocument);

            // Документ не найден
            if (result.MatchedCount == 0)
                throw new KeyNotFoundException($"Документ с id={id} не найден");

            // Данные не изменились, но это не ошибка
            var updatedDocument = await collection.Find(filter).FirstOrDefaultAsync();

            var bsonDoc = updatedDocument.ToDictionary();
            bsonDoc["_id"] = updatedDocument["_id"].ToString();

            return new DynamicResourceData
            {
                Data = JsonNode.Parse(bsonDoc.ToJson())!
            };
        }
    }
}
