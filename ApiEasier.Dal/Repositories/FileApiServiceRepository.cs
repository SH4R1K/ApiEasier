using ApiEasier.Dal.Interfaces;
using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Repositories
{
    public class FileApiServiceRepository : IFileApiServiceRepository
    {
        public Task AddAsync(ApiService apiService)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ApiService apiService)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApiService>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApiService?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ApiService apiService)
        {
            throw new NotImplementedException();
        }
    }
}
