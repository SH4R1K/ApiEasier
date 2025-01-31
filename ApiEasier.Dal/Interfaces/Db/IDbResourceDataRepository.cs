using ApiEasier.Dal.Helpers;
using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Interfaces.Db
{
    public interface IDbResourceDataRepository
    {
        Task<List<DynamicCollectionModel>?> GetAllDataAsync(string resourceName);
        Task<DynamicCollectionModel> CreateDataAsync(string resourceName, object data);
        Task<DynamicCollectionModel> UpdateDataAsync(string resourceName, object data);
        Task<bool> DeleteDataAsync(string resourceName, string id);
        Task<DynamicCollectionModel> GetDataByIdAsync(string resourceName, string id);
    }
}
