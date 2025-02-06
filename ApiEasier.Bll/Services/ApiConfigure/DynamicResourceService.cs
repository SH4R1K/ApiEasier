using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Dal.Interfaces.Db;

namespace ApiEasier.Bll.Services.ApiConfigure
{
    public class DynamicResourceService : IDynamicResourceService
    {
        private readonly IDbResourceRepository _dbResourceRepository;
        public DynamicResourceService(IDbResourceRepository dbResourceRepository)
        {
            _dbResourceRepository = dbResourceRepository;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var resourcesNames = await _dbResourceRepository.GetNamesAsync();

                var resourcesToDelete = resourcesNames.Where(r => r.StartsWith(id + "_")).ToList();

                var tasks = resourcesToDelete.Select(async resourceName =>
                {
                    await _dbResourceRepository.DeleteAsync(resourceName);
                });

                await Task.WhenAll(tasks);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateNameAsync(string resourceName, string newResourceName)
        {
            try
            {
                var resourceNames = await _dbResourceRepository.GetNamesAsync();

                var collectionsToUpdate = resourceNames.Where(name => name.StartsWith(resourceName + "_")).ToList();

                // Асинхронное обновление коллекций параллельно
                var tasks = collectionsToUpdate.Select(name => _dbResourceRepository.UpdateNameAsync(name, newResourceName));
                await Task.WhenAll(tasks);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during update: {ex.Message}");
                return false;
            }
        }
    }
}
