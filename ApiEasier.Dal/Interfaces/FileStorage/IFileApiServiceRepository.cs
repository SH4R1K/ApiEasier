using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Interfaces.FileStorage
{
    public interface IFileApiServiceRepository
    {
        Task<bool> CreateAsync(ApiService apiService);
        Task<bool> UpdateAsync(string id, ApiService apiService);
        bool Delete(string id);
        Task<List<ApiService>> GetAllAsync();
        Task<ApiService?> GetByIdAsync(string id);
    }
}
