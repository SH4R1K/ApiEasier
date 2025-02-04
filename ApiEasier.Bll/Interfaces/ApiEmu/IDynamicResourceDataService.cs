using ApiEasier.Dm.Models.Dynamic;

namespace ApiEasier.Bll.Interfaces.ApiEmu
{
    public interface IDynamicResourceDataService
    {
        Task<DynamicResource> AddDataAsync(string apiName, string apiEntityName, object jsonData);

        Task<List<DynamicResource>> GetDataAsync(string apiName, string apiEntityName, string? filters);

        Task<DynamicResourceData> GetDataByIdAsync(string apiName, string apiEntityName, string id, string? filters);

        Task<bool> DeleteDataAsync(string apiName, string apiEntityName, string id);

        Task<DynamicResource> UpdateDataAsync(string apiName, string apiEntityName, string id, object jsonData);
    }
}
