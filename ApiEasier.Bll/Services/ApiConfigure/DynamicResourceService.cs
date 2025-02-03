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
                var apiServices = await _dbResourceRepository.GetNamesAsync();

                foreach (var apiService in apiServices)
                {
                    if (apiService.StartsWith(id))
                    {
                        await _dbResourceRepository.DeleteAsync(apiService + "_" + id.Trim());
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
