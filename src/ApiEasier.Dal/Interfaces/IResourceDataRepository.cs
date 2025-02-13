using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Interfaces
{
    public interface IResourceDataRepository
    {
        Task<List<DynamicResourceData>?> GetAllDataAsync(string resourceName);
        Task<DynamicResourceData> CreateDataAsync(string resourceName, object data);
        Task<DynamicResourceData?> UpdateDataAsync(string resourceName, string id, object data);
        Task<bool> DeleteDataAsync(string resourceName, string id);
        Task<DynamicResourceData?> GetDataByIdAsync(string resourceName, string id);
    }
}
