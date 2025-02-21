using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Interfaces
{
    /// <summary>
    /// Обеспечивает работу с API-сервисами
    /// </summary>
    public interface IApiServiceRepository
    {
        /// <summary>
        /// Добавляет новый API-сервис
        /// </summary>
        /// <param name="apiService">Создаваемый API-сервис</param>
        /// <returns>Созданный API-сервис</returns>
        Task<ApiService?> CreateAsync(ApiService apiService);

        /// <summary>
        /// Изменяет данные API-сервиса
        /// </summary>
        /// <param name="id">Идентификатор API-сервиса</param>
        /// <param name="apiService">Новые данные для API-сервиса</param>
        /// <returns>Изменненый API-сервис</returns>
        Task<ApiService?> UpdateAsync(string id, ApiService apiService);

        /// <summary>
        /// Удаляет API-сервис
        /// </summary>
        /// <param name="id">Идентификатор API-сервиса</param>
        Task DeleteAsync(string id);

        /// <summary>
        /// Возвращает перечень всех API-сервисов
        /// </summary>
        /// <returns>Список API-сервисов</returns>
        Task<List<ApiService>> GetAllAsync();

        /// <summary>
        /// Возвращает API-сервиса по имени
        /// </summary>
        /// <param name="id">Идентификатор API-сервиса</param>
        /// <returns>Требуемый API-сервис</returns>
        Task<ApiService?> GetByIdAsync(string id);

        /// <summary>
        /// Меняет активность API-сервисов
        /// </summary>
        /// <param name="status">True если надо сделать активным API-сервис, false - неактивным</param>
        /// <param name="id">Имя API-сервиса</param>
        Task ChangeActiveStatusAsync(string id, bool status);
    }
}
