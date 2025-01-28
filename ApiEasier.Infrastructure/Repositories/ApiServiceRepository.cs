using ApiEasier.Domain.Interfaces;
using ApiEasier.Server.Db;
using ApiEasier.Server.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ApiEasier.Infrastructure.Repositories
{
    public class ApiServiceRepository : IApiServiceRepository
    {
        private readonly IFileRepository _fileRepository;

        public ApiServiceRepository(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public Task AddAsync(ApiService service)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ApiService service)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApiService>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiService> GetByIdAsync(string id)
        {
            var apiService = await _fileRepository.ReadFromFileAsync<ApiService>();
            return apiService;
        }

        public Task UpdateAsync(ApiService service)
        {
            throw new NotImplementedException();
        }
    }
}
