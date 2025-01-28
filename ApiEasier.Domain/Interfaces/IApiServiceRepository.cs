using ApiEasier.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEasier.Domain.Interfaces
{
    public interface IApiServiceRepository
    {
        Task AddAsync(ApiService apiService);
        Task UpdateAsync(ApiService apiService);
        Task DeleteAsync(ApiService apiService);
        Task<IEnumerable<ApiService>> GetAllAsync();
        Task<ApiService?> GetByIdAsync(string id);
    }
}
