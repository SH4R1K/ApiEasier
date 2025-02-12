using ApiEasier.Dal.Data;
using ApiEasier.Dal.Interfaces.Db;


namespace ApiEasier.Dal.Repositories.Db
{
    public class DbResourceRepository : IDbResourceRepository
    {
        private readonly MongoDbContext _dbContext;

        public DbResourceRepository(MongoDbContext context)
        {
            _dbContext = context;
        }

        public async Task<bool> DeleteByApiEntityNameAsync(string id)
        {
            var collectionsNames = await _dbContext.GetListCollectionNamesAsync();

            var collectionsToDelete = collectionsNames.Where(name => name.EndsWith("_" + id)).ToList();

            if (collectionsToDelete.Count == 0)
                return false;

            var deleteTasks = collectionsToDelete.Select(_dbContext.DropCollectionAsync);

            await Task.WhenAll(deleteTasks);
            return true;
        }

        public async Task<bool> DeleteByApiNameAsync(string id)
        {
            var collectionsNames = await _dbContext.GetListCollectionNamesAsync();

            var collectionsToDelete = collectionsNames.Where(name => name.StartsWith(id + "_")).ToList();

            if (collectionsToDelete.Count == 0)
                return false;

            var deleteTasks = collectionsToDelete.Select(_dbContext.DropCollectionAsync);

            await Task.WhenAll(deleteTasks);
            return true;
        }

        public async Task<bool> UpdateByApiEntityNameAsync(string apiServiceName, string id, string newId)
        {
            try
            {
                await _dbContext.RenameCollectionAsync(apiServiceName + "_" + id, apiServiceName + "_" + newId);
                return true;
            }
            catch
            {
                return false;
            }      
        }

        public async Task<bool> UpdateByApiNameAsync(string id, string newId)
        {
            var collectionsNames = await _dbContext.GetListCollectionNamesAsync();

            var collectionsToUpdate = collectionsNames.Where(name => name.StartsWith(id + "_")).ToList();

            if (collectionsToUpdate.Count == 0)
                return false;

            var updateTask = collectionsToUpdate.Select(oldName =>
            {
                var newName = oldName.Replace(id + "_", newId + "_");
                return _dbContext.RenameCollectionAsync(oldName, newName);
            });

            await Task.WhenAll(updateTask);

            return true;
        }
    }
}
