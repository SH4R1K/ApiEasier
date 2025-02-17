using ApiEasier.Bll.Dto;

namespace ApiEasier.Bll.Interfaces.ApiConfigure
{
    /// <summary>
    /// Обеспечивает настройку api-сервисов
    /// </summary>
    public interface IDynamicApiConfigurationService
    {
        /// <summary>
        /// Добавляет новый API-сервис
        /// </summary>
        /// <param name="apiServiceDto">Создаваемый API-сервис</param>
        /// <returns>Созданный API-сервис</returns>
        Task<ApiServiceDto?> CreateAsync(ApiServiceDto apiServiceDto);

        /// <summary>
        /// Изменяет данные API-сервиса
        /// </summary>
        /// <param name="id">Идентификатор API-сервиса</param>
        /// <param name="apiServiceDto">Новые данные для API-сервиса</param>
        /// <returns>Изменненый API-сервис</returns>
        Task<ApiServiceDto?> UpdateAsync(string id, ApiServiceDto apiServiceDto);
      
        /// <summary>
        /// Возвращает перечень всех API-сервисов без сущностей
        /// </summary>
        /// <returns>Список API-сервисов без сущностей</returns>
        Task<List<ApiServiceSummaryDto>> GetAllAsync();
        Task<bool >DeleteAsync(string id);

        /// <summary>
        /// Возвращает API-сервиса по имени
        /// </summary>
        /// <param name="id">Идентификатор API-сервиса</param>
        /// <returns>Требуемый API-сервис</returns>
        Task<ApiServiceDto?> GetByIdAsync(string id);
        Task<bool> ChangeActiveStatusAsync(string id, bool status);
    }
}
