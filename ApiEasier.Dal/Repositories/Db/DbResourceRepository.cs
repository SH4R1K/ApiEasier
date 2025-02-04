using ApiEasier.Dal.Data;
using ApiEasier.Dal.Interfaces.Db;
using ApiEasier.Dm.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEasier.Dal.Repositories.Db
{
    public class DbResourceRepository : IDbResourceRepository
    {
        private readonly MongoDbContext _dbContext;

        public DbResourceRepository(MongoDbContext context)
        {
            _dbContext = context;
        }

        public async Task<bool> DeleteAsync(string resourceName)
        {
            var result = await _dbContext.DropCollectionAsync(resourceName);
            return result;
        }

        public async Task<List<string>> GetNamesAsync()
        {
            var result = await _dbContext.GetListCollectionNamesAsync();
            return result;

        }

        public async Task UpdateNameAsync(string oldName, string name)
        {
            await _dbContext.RenameCollectionAsync(oldName, name);
        }
    }
}
