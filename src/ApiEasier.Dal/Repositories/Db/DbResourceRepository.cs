using ApiEasier.Dal.Data;
using ApiEasier.Dal.Interfaces;

namespace ApiEasier.Dal.Repositories.Db
{
    public class DbResourceRepository(MongoDbContext context) : IResourceRepository
    {
        private readonly MongoDbContext _dbContext = context;

        /// <summary>
        /// Находит коллекции, привязанные к API-сервису, и удаляет их
        /// </summary>
        /// <param name="id">Имя API-сервиса</param>
        /// <inheritdoc/>
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

        /// <summary>
        /// Находит коллекции, привязанные к API-сервису, и переименовывает их, чтобы она 
        /// ссылалась на переименованный API-сервис
        /// </summary>
        /// <param name="id">Текущее имя API-сервиса</param>
        /// <param name="newId">Новое имя API-сервиса</param>
        /// <inheritdoc/>
        public async Task<bool> UpdateByApiNameAsync(string id, string newId)
        {
            var collectionsNames = await _dbContext.GetListCollectionNamesAsync();

            // Имя коллекции ИмяApiСервиса_ИмяСущности
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

        /// <summary>
        /// Удаляет все коллекции, которые не начинаются с имени API-сервиса
        /// </summary>
        /// <inheritdoc/>
        public async Task DeleteUnusedResources(List<string> apiServiceNames)
        {
            // коллекции хранятся в виде: ApiServiceName_ApiEntityName
            var collectionsNames = await _dbContext.GetListCollectionNamesAsync();

            var collectionToDelete = collectionsNames.Where(name =>
            {
                var apiName = name.Split("_")[0];

                return !apiServiceNames.Contains(apiName);
            }).ToList();

            var deleteTasks = collectionToDelete.Select(_dbContext.DropCollectionAsync);
            await Task.WhenAll(deleteTasks);
        }
    }
}
