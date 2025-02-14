using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Interfaces
{
    /// <summary>
    /// Обеспечивает работу с API-сервисами
    /// </summary>
    public interface IApiServiceRepository
    {
        Task<ApiService?> CreateAsync(ApiService apiService);
        Task<ApiService?> UpdateAsync(string id, ApiService apiService);
        bool Delete(string id);

        /// <summary>
        /// Возвращает перечень всех API-сервисов
        /// </summary>
        /// <returns>Список API-сервисов</returns>
        Task<List<ApiService>> GetAllAsync();

        /// <summary>
        /// Возвращает API-сервиса по имени
        /// </summary>
        /// <param name="id">Имя API-сервиса</param>
        /// <returns>Требуемый API-сервис</returns>
        Task<ApiService?> GetByIdAsync(string id);
        Task<bool> ChangeActiveStatusAsync(string id, bool status);
    }
}
