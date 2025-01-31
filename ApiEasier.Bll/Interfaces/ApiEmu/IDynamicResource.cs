using ApiEasier.Dal.Helpers;

namespace ApiEasier.Bll.Interfaces.ApiEmu
{
    /// <summary>
    /// Интерфейс для работы с динамическими коллекциями в MongoDB.
    /// </summary>
    public interface IDynamicResource
    {

        Task<DynamicCollectionModel> AddDataAsync(string apiName, string apiEntityName, object jsonData);

        Task<List<DynamicCollectionModel>> GetDataAsync(string apiName, string apiEntityName, string? filters);

        Task<DynamicCollectionModel> GetDataByIdAsync(string apiName, string apiEntityName, string id, string? filters);
    }
}
