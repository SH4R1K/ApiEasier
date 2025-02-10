using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Interfaces.FileStorage
{
    public interface IFileApiEndpointRepository
    {
        Task<List<ApiEndpoint>> GetAllAsync(string apiServiceName, string apiEntityName);
        Task<ApiEndpoint?> GetByIdAsync(string apiServiceName, string apiEntityName, string id);
        Task<bool> CreateAsync(string apiServiceName, string apiEntityName, ApiEndpoint apiEndpoint);
        Task<bool> UpdateAsync(string apiServiceName, string apiEntityName, string id, ApiEndpoint apiEndpoint);
        Task<bool> DeleteAsync(string apiServiceName, string apiEntityName, string id);
        Task<bool> ChangeActiveStatusAsync(string apiServiceName, string apiEntityName, string id, bool status);
    }
}
