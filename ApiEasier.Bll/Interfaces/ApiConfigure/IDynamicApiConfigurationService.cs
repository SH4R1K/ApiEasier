using ApiEasier.Bll.Dto;

namespace ApiEasier.Bll.Interfaces.ApiConfigure
{
    public interface IDynamicApiConfigurationService
    {
        Task<bool> CreateAsync(ApiServiceDto dto);
        Task<ApiServiceDto?> UpdateAsync(string id, ApiServiceDto dto);
        bool Delete(string id);
        Task<List<ApiServiceDto>> GetAsync();
        Task<ApiServiceDto?> GetByIdAsync(string id);
        Task<bool> ChangeActiveStatusAsync(string id, bool status);
    }
}
