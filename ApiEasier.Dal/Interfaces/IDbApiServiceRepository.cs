using ApiEasier.Dal.Helpers;
using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Interfaces
{
    public interface IDbApiServiceRepository
    {
        Task<DynamicApiService> GetApiServiceData(string apiServiceName);
    }
}
