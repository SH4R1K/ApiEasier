using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Interfaces.FileStorage
{
    public interface IFileApiServiceRepository
    {
        Task<ApiService?> CreateAsync(ApiService apiService);
        Task<ApiService?> UpdateAsync(string id, ApiService apiService);
        bool Delete(string id);
        Task<List<ApiService>> GetAllAsync();
        Task<ApiService?> GetByIdAsync(string id);
        Task<bool> ChangeActiveStatusAsync(string id, bool status);
    }
}
