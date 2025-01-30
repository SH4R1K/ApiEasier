using ApiEasier.Dal.Helpers;
using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Interfaces
{
    public interface IDbApiServiceRepository
    {
        Task<List<DynamicCollectionModel>?> GetDataAsync(string apiServiceName);
        Task<DynamicCollectionModel> CreateAsync(string apiServiceName, object data);
        Task<DynamicCollectionModel> UpdateAsync(string apiServiceName, object data);
        Task<bool> DeleteAsync(string apiServiceName);
        Task<DynamicCollectionModel> GetDataByIdAsync(string apiServiceName, string id);
    }
}
