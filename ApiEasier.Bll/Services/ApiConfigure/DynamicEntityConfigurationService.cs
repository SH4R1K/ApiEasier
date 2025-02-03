using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Dal.Interfaces.Db;
using ApiEasier.Dal.Interfaces.FileStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEasier.Bll.Services.ApiConfigure
{
    public class DynamicEntityConfigurationService : IDynamicEntityConfigurationService
    {
        private readonly IFileApiServiceRepository _fileApiServiceRepository;
        private readonly IDbResourceRepository _dbResourceRepository;

        public DynamicEntityConfigurationService(
            IFileApiServiceRepository fileApiServiceRepository,
            IDbResourceRepository dbResourceRepository)
        {
            _fileApiServiceRepository = fileApiServiceRepository;
            _dbResourceRepository = dbResourceRepository;
        }

        public Task<bool> DeleteAsync(string apiServiceName, string id)
        {
            string resourceName = apiServiceName.Trim().Replace(" ", "") + "_" + id.Trim().Replace(" ", "");

            _dbResourceRepository.DeleteAsync(resourceName);

            _fileApiServiceRepository.Delete(id);
        }

        public Task<bool> UpdateAsync(string apiServiceName, ApiEntityDto entity)
        {
            throw new NotImplementedException();
        }

        Task<bool> IDynamicEntityConfigurationService.CreateAsync(string apiServiceName, ApiEntityDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
