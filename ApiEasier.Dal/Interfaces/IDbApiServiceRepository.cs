using ApiEasier.Dal.Helpers;
using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Interfaces
{
    public interface IDbApiServiceRepository
    {
        Task<List<DynamicApiServiceModel>?> GetDataAsync(string apiServiceName);
        Task<DynamicApiServiceModel> CreateAsync(dynamic apiServiceName, object data);
    }
}
