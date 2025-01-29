using ApiEasier.Dal.DB;
using ApiEasier.Dal.Helpers;
using ApiEasier.Dal.Interfaces;
using ApiEasier.Dm.Models;
using MongoDB.Bson;
using System.Collections;

namespace ApiEasier.Dal.Repositories
{
    public class DbApiServiceRepository : IDbApiServiceRepository
    {
        private readonly MongoDbContext _dbContext;

        public DbApiServiceRepository(MongoDbContext context)
        {
            _dbContext = context;
        }

        public Task<DynamicApiService> GetApiServiceData(string apiServiceName)
        {
            var data = _dbContext.GetCollection<BsonDocument>(apiServiceName);

            var result = data.Select(doc => new DynamicApiService
            {
                Name = doc["_id"].AsObjectId,
                Data = doc.ToDictionary()
            });

            return result ?? null;
        }
    }
}
