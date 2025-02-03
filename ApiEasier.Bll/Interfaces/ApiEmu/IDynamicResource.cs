using ApiEasier.Dal.Helpers;

namespace ApiEasier.Bll.Interfaces.ApiEmu
{
    public interface IDynamicResource
    {
        Task<DynamicResourceModel> AddDataAsync(string apiName, string apiEntityName, object jsonData);

        Task<List<DynamicResourceModel>> GetDataAsync(string apiName, string apiEntityName, string? filters);

        Task<DynamicResourceDataModel> GetDataByIdAsync(string apiName, string apiEntityName, string id, string? filters);

        Task<bool> DeleteDataAsync(string apiName, string apiEntityName, string id);

        Task<DynamicResourceModel> UpdateDataAsync(string apiName, string apiEntityName, string id, object jsonData);
    }
}
