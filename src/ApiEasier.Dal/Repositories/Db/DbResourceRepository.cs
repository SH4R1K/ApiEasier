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

        public async Task<bool> DeleteAsync(string id)
        {
            var collectionsNames = await _dbContext.GetListCollectionNamesAsync();

            var collectionsToDelete = collectionsNames.Where(name => name.StartsWith(id + "_")).ToList();

            if (collectionsToDelete.Count == 0)
                return false;

            var deleteTasks = collectionsToDelete.Select(_dbContext.DropCollectionAsync);

            await Task.WhenAll(deleteTasks);
            return true;
        }
    }
}
