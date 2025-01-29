using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Interfaces
{
    public interface IFileApiServiceRepository
    {
        Task AddAsync(ApiService apiService);
        Task UpdateAsync(string id, ApiService apiService);
        void Delete(ApiService apiService);
        Task<List<ApiService>> GetAllAsync();
        Task<ApiService?> GetByIdAsync(string id);
    }
}
