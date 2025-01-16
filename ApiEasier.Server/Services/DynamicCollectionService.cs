using ApiEasier.Server.DB;
using ApiEasier.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiEasier.Server.Services
{
    public class DynamicCollectionService : IDynamicCollectionService
    {
        private readonly MongoDBContext _dbContext;

        public DynamicCollectionService(MongoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddDocToCollectionAsync(string collectionName, object jsonData)
        {
            try
            {
                var collecntion = _dbContext.GetCollection<BsonDocument>(collectionName);
                var bsonDocument = BsonDocument.Parse(jsonData.ToString());
                await collecntion.InsertOneAsync(bsonDocument);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error");
            }
        }

        public async Task<List<Dictionary<string, object>>> GetDocFromCollectionAsync(string collectionName)
        {
            try
            {
                var collection = _dbContext.GetCollection<BsonDocument>(collectionName);
                var documents = await collection.Find(_ => true).ToListAsync();

                // Преобразуем BsonDocument в Dictionary<string, object> для корректного преобразования Bson в Json через Dictionary
                var jsonList = documents.Select(doc =>
                {
                    var bsonDoc = doc.ToDictionary();
                    bsonDoc["_id"] = doc["_id"].ToString(); // Преобразуем ObjectId в строку
                    return bsonDoc;
                }).ToList();

                return jsonList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw new ArgumentException("Failed to retrieve documents from the collection.");
            }
        }
    }
}
