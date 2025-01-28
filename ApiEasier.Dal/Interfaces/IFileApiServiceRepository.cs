using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Interfaces
{
    public interface IFileApiServiceRepository
    {
        Task AddAsync(ApiService apiService);
        Task UpdateAsync(ApiService apiService);
        Task DeleteAsync(ApiService apiService);
        Task<IEnumerable<ApiService>> GetAllAsync();
        Task<ApiService?> GetByIdAsync(string id);
    }
}
