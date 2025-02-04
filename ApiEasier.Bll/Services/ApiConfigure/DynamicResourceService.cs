using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Dal.Interfaces.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                var resourceNames = await _dbResourceRepository.GetNamesAsync();

                foreach (var name in resourceNames)
                {
                    if (name.StartsWith(id))
                    {
                        await _dbResourceRepository.DeleteAsync(name);
                    }
                }
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
