using ApiEasier.Dal.Helpers;
using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Interfaces.Db
{
    public interface IDbResourceDataRepository
    {
        Task<List<DynamicResourceModel>?> GetAllDataAsync(string resourceName);
        Task<DynamicResourceModel> CreateDataAsync(string resourceName, object data);
        Task<DynamicResourceModel> UpdateDataAsync(string resourceName, string id, object data);
        Task<bool> DeleteDataAsync(string resourceName, string id);
        Task<DynamicResourceDataModel> GetDataByIdAsync(string resourceName, string id);
    }
}
