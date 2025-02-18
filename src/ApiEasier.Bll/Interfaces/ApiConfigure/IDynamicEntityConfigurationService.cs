using ApiEasier.Bll.Dto;

namespace ApiEasier.Bll.Interfaces.ApiConfigure
{
    /// <summary>
    /// Позволяет управлять сущностями API-сервиса
    /// </summary>
    public interface IDynamicEntityConfigurationService
    {
        /// <summary>
        /// Возвращает все сущности API-сервиса
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса</param>
        /// <returns>Список всех сущностей API-сервиса</returns>
        Task<List<ApiEntitySummaryDto>?> GetAllAsync(string apiServiceName);
        Task<ApiEntityDto?> GetByIdAsync(string apiServiceName, string id);
        Task<ApiEntityDto?> CreateAsync(string apiServiceName, ApiEntityDto entity);
        Task<bool> UpdateAsync(string apiServiceName,string entityName, ApiEntityDto entity);
        Task<bool> DeleteAsync(string apiServiceName, string id);
        public Task<bool> ChangeActiveStatusAsync(string apiServiceName, string entityName, bool status);
    }
}
