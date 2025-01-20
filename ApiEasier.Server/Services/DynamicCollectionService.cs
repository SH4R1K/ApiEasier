using ApiEasier.Server.DB;
using ApiEasier.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections;
using System.Reflection.Metadata;

namespace ApiEasier.Server.Services
{
    public class DynamicCollectionService : IDynamicCollectionService
    {
        private readonly MongoDBContext _dbContext;

        public DynamicCollectionService(MongoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Dictionary<string, object>> AddDocToCollectionAsync(string collectionName, object jsonData)
        {
            try
            {
                var collection = _dbContext.GetCollection<BsonDocument>(collectionName);
                var bsonDocument = BsonDocument.Parse(jsonData.ToString());
                await collection.InsertOneAsync(bsonDocument);

                var bsonDoc = bsonDocument.ToDictionary();
                bsonDoc["_id"] = bsonDocument["_id"].ToString();
                return bsonDoc;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw new ArgumentException($"{ex}");
            }
        }

        public async Task<long?> DeleteDocFromCollectionAsync(string collectionName, string id)
        {
            try
            {
                var collections = await _dbContext.ListCollectionNamesAsync();
                if (!collections.Contains(collectionName))
                    return null;

                var collection = _dbContext.GetCollection<BsonDocument>(collectionName);
                var objectId = new ObjectId(id);
                var document = await collection.DeleteOneAsync(Builders<BsonDocument>.Filter.Eq("_id", objectId));
               
                return document.DeletedCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw new ArgumentException("err");
            }
        }

        public async Task<Dictionary<string, object>?> GetDocByIdFromCollectionAsync(string collectionName, string id, string? filters)
        {
            try
            {
                var collections = await _dbContext.ListCollectionNamesAsync();
                if (!collections.Contains(collectionName))
                    return null;

                var collection = _dbContext.GetCollection<BsonDocument>(collectionName);


                // Берём документ по id из коллекции
                var objectId = new ObjectId(id);
                var idFilter = Builders<BsonDocument>.Filter.Eq("_id", objectId);
                var documentFilter = BsonSerializer.Deserialize<BsonDocument>(filters);
                var combineFilter = Builders<BsonDocument>.Filter.And(idFilter, documentFilter);
                var document = await collection.Find(combineFilter).FirstOrDefaultAsync();
                if (document == null)
                    return null;

                var bsonDoc = document.ToDictionary();
                bsonDoc["_id"] = document["_id"].ToString();
                return bsonDoc;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw new ArgumentException("err");
            }
        }

        public async Task<List<Dictionary<string, object>>?> GetDocFromCollectionAsync(string collectionName, string? filters)
        {
            try
            {
                var collections = await _dbContext.ListCollectionNamesAsync();
                if (!collections.Contains(collectionName))
                    return null;

                var collection = _dbContext.GetCollection<BsonDocument>(collectionName);

                FilterDefinition<BsonDocument> filterDefinition;
                if (string.IsNullOrEmpty(filters))
                {
                    // Если filters null или пустой, возвращаем все документы
                    filterDefinition = Builders<BsonDocument>.Filter.Empty;
                }
                else
                {
                    // Преобразуем строку фильтров в BsonDocument
                    filterDefinition = BsonSerializer.Deserialize<BsonDocument>(filters);
                }
                // Берём все документы из коллекции
                var documents = await collection.Find(filterDefinition).ToListAsync();

                // Преобразуем BsonDocument в Dictionary<string, object> для корректного преобразования Bson в Json через Dictionary
                var jsonList = documents.Select(doc =>
                {
                    var bsonDoc = doc.ToDictionary();
                    bsonDoc["_id"] = doc["_id"].ToString(); // Преобразуем ObjectId в строку для корректного отображения
                    return bsonDoc;
                }).ToList();

                return jsonList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw new ArgumentException("err");
            }
        }

        public async Task<Dictionary<string, object>?> UpdateDocFromCollectionAsync(string collectionName, object jsonData)
        {
            try
            {
                var collections = await _dbContext.ListCollectionNamesAsync();
                if (!collections.Contains(collectionName))
                    return null;

                var collection = _dbContext.GetCollection<BsonDocument>(collectionName);

                var bsonDocument = BsonDocument.Parse(jsonData.ToString());

                if (!bsonDocument.Contains("_id"))
                    return null;

                var objectId = new ObjectId(bsonDocument["_id"].ToString());
                bsonDocument.Remove("_id"); // Удаляем _id из данных, чтобы не перезаписать его

                // Используем replaceOne для замены всего документа
                var replacementResult = await collection.ReplaceOneAsync(
                    Builders<BsonDocument>.Filter.Eq("_id", objectId),
                    bsonDocument
                );

                if (replacementResult.ModifiedCount > 0)
                {
                    // Если документ заменен, возвращаем обновленный документ
                    var updatedDocument = await collection.Find(Builders<BsonDocument>.Filter.Eq("_id", objectId)).FirstOrDefaultAsync();
                    if (updatedDocument != null)
                    {
                        var bsonDoc = updatedDocument.ToDictionary();
                        bsonDoc["_id"] = updatedDocument["_id"].ToString();
                        return bsonDoc;
                    }
                }

                return null; // Если документ не был заменен
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw new ArgumentException("err");
            }
        }
    }
}
