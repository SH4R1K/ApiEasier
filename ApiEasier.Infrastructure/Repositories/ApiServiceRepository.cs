using ApiEasier.Domain.Interfaces;
using ApiEasier.Server.Models;


namespace ApiEasier.Infrastructure.Repositories
{
    public class ApiServiceRepository : IApiServiceRepository
    {
        private readonly IJsonRepository _jsonRepository;

        public ApiServiceRepository(IJsonRepository jsonRepository)
        {
            _jsonRepository = jsonRepository;
        }

        public async Task AddAsync(ApiService service)
        {
            await _jsonRepository.WriteJsonAsync<ApiService>(service.Name, service);
        }

        public Task DeleteAsync(ApiService service)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApiService>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Дессериализация данных из файла json в инстанс ApiSerive
        /// </summary>
        /// <param name="id">название файла</param>
        /// <returns></returns>
        public async Task<ApiService?> GetByIdAsync(string id)
        {
            var apiService = await _jsonRepository.ReadJsonAsync<ApiService>(id);
            apiService.Name = id;
            return apiService;
        }

        public Task UpdateAsync(ApiService service)
        {
            throw new NotImplementedException();
        }
    }
}
