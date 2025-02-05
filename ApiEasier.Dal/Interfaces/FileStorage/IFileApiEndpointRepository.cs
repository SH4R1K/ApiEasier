using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Interfaces.FileStorage
{
    public interface IFileApiEndpointRepository
    {
        Task<bool> CreateAsync(string apiServiceName, string apiEntityName, ApiEndpoint apiEndpoint);
        Task<bool> UpdateAsync(string apiServiceName, string apiEntityName, string id, ApiEndpoint apiEndpoint);
        Task<bool> DeleteAsync(string apiServiceName, string apiEntityName, string id);
        Task<bool> ChangeActiveStatusAsync(string apiServiceName, string apiEntityName, string id, bool status);
    }
}
