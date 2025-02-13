using ApiEasier.Bll.Dto;

namespace ApiEasier.Bll.Interfaces.ApiConfigure
{
    /// <summary>
    /// Обеспечивает настройку api-сервисов
    /// </summary>
    public interface IDynamicApiConfigurationService
    {
        Task<ApiServiceDto?> CreateAsync(ApiServiceDto dto);
        Task<ApiServiceDto?> UpdateAsync(string id, ApiServiceDto dto);
      
        /// <summary>
        /// Возвращает перечень всех API-сервисов без сущностей
        /// </summary>
        /// <returns>Список API-сервисов без сущностей</returns>
        Task<List<ApiServiceSummaryDto>> GetAllAsync();
        Task<bool >DeleteAsync(string id);
        Task<ApiServiceDto?> GetByIdAsync(string id);
        Task<bool> ChangeActiveStatusAsync(string id, bool status);
    }
}
