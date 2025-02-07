using ApiEasier.Bll.Dto;

namespace ApiEasier.Bll.Interfaces.ApiConfigure
{
    public interface IDynamicEndpointConfigurationService
    {
        Task<List<ApiEndpointDto>> GetAsync(string apiServiceName, string apiEntityName);
        Task<ApiEndpointDto?> GetByIdAsync(string apiServiceName, string apiEntityName, string id);
        Task<bool> CreateAsync(string apiServiceName, string apiEntityName, ApiEndpointDto apiEndpointDto);
        Task<bool> UpdateAsync(string apiServiceName, string apiEntityName, string apiEndpointName, ApiEndpointDto apiEndpointDto);
        Task<bool> DeleteAsync(string apiServiceName, string apiEntityName, string id);
        public Task<bool> ChangeActiveStatusAsync(string apiServiceName, string entityName, string endpointName, bool status);
    }
}
