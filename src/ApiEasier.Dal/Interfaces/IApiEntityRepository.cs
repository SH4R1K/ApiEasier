using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Interfaces
{
    /// <summary>
    /// Позволяет совершать операции над сущностями
    /// </summary>
    public interface IApiEntityRepository
    {
        /// <summary>
        /// Возвращает все сущности API-сервиса
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса</param>
        /// <returns>Список всех сущностей API-сервиса</returns>
        Task<List<ApiEntity>?> GetAllAsync(string apiServiceName);
        Task<ApiEntity?> GetByIdAsync(string apiServiceName, string id);
        Task<ApiEntity?> CreateAsync(string apiServiceName, ApiEntity apiEntity);
        Task<bool> UpdateAsync(string apiServiceName, string id, ApiEntity apiEntity);
        Task<bool> DeleteAsync(string apiServiceName, string id);
        Task<bool> ChangeActiveStatusAsync(string apiServiceName, string id, bool status);
    }
}
