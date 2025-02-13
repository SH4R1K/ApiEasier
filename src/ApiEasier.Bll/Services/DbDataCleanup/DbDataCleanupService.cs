using ApiEasier.Bll.Interfaces.DbDataCleanup;
using ApiEasier.Dal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEasier.Bll.Services.DbDataCleanup
{
    public class DbDataCleanupService : IDbDataCleanupService
    {
        private readonly IResourceRepository _dbResourceRepository;
        private readonly IApiServiceRepository _fileApiServiceRepository;
        

        public DbDataCleanupService(
            IResourceRepository dbResourceRepository,
            IApiServiceRepository fileApiServiceRepository)
        {
            _fileApiServiceRepository = fileApiServiceRepository;
            _dbResourceRepository = dbResourceRepository;
        }

        public async Task CleanupAsync()
        {
            var apiServices = await _fileApiServiceRepository.GetAllAsync();

            List<string> apiServiceNames = apiServices.Select(a => a.Name).ToList();

            await _dbResourceRepository.DeleteUnusedResources(apiServiceNames);
        }

    }
}
