using ApiEasier.Server.DB;
using ApiEasier.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ApiEasier.Server.Services
{
    public class DynamicCollectionService : IDynamicCollectionService
    {
        private readonly MongoDBContext _dbContext;

        public DynamicCollectionService(MongoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async  Task AddToCollectionAsync(string collectionName, object jsonData)
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
    }
}
