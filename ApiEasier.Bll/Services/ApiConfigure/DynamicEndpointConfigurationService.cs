using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Dal.Interfaces.FileStorage;

namespace ApiEasier.Bll.Services.ApiConfigure
{
    public class DynamicEndpointConfigurationService : IDynamicEndpointConfigurationService
    {
        private readonly IFileApiEndpointRepository _fileApiEndpointRepository;

        public DynamicEndpointConfigurationService(IFileApiEndpointRepository fileApiEndpointRepository)
        {
            _fileApiEndpointRepository = fileApiEndpointRepository;
        }

        public async Task<bool> ChangeActiveStatusAsync(string apiServiceName, string entityName, string endpointName, bool status)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateAsync(string apiServiceName, string apiEntityName, ApiEndpointDto apiEndpointDto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(string apiServiceName, string apiEntityName, string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(string apiServiceName, string apiEntityName, ApiEndpointDto apiEndpointDto)
        {
            throw new NotImplementedException();
        }
    }
}
