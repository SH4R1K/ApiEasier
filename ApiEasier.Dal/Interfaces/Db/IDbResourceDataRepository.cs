using ApiEasier.Dm.Models.Dynamic;

namespace ApiEasier.Dal.Interfaces.Db
{
    public interface IDbResourceDataRepository
    {
        Task<List<DynamicResource>> GetAllDataAsync(string resourceName);
        Task<DynamicResource> CreateDataAsync(string resourceName, object data);
        Task<DynamicResource> UpdateDataAsync(string resourceName, string id, object data);
        Task<bool> DeleteDataAsync(string resourceName, string id);
        Task<DynamicResource> GetDataByIdAsync(string resourceName, string id);
    }
}
