using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Interfaces
{
    /// <summary>
    /// Позволяет управлять эндпоинтами сущности
    /// </summary>
    public interface IApiEndpointRepository
    {
        /// <summary>
        /// Возвращет все эндпоинты указанной сущности
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="apiEntityName">Имя сущности с требуемыми эндпоинтами</param>
        /// <returns>Список эндпоинтов указанной сущности</returns>
        Task<List<ApiEndpoint>> GetAllAsync(string apiServiceName, string apiEntityName);

        /// <summary>
        /// Возвращает эндпоинт указанной сущности по имени
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="apiEntityName">Имя сущности с указанным эндпоинтом</param>
        /// <param name="id">Идентификатор требуемого эндпоинта</param>
        /// <returns>Требуемый эндпоинт</returns>
        Task<ApiEndpoint?> GetByIdAsync(string apiServiceName, string apiEntityName, string id);

        /// <summary>
        /// Создает эндпоинт в указанной сущности
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="apiEntityName">Имя сущности с указанным эндпоинтом</param>
        /// <param name="apiEndpointDto">Данные нового эндпоинта</param>
        /// <returns>Созданнный эндпоинт</returns>
        Task<ApiEndpoint?> CreateAsync(string apiServiceName, string apiEntityName, ApiEndpoint apiEndpoint);

        /// <summary>
        /// Изменяет эндпоинт у указанной сущности
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="apiEntityName">Имя сущности с указанным эндпоинтом</param>
        /// <param name="id">Идентификатор изменяемого эндпоинта</param>
        /// <param name="apiEndpoint">Новые данные эндпоинта</param>
        /// <returns>Изменненый эндпоинт</returns>
        Task<ApiEndpoint> UpdateAsync(string apiServiceName, string apiEntityName, string id, ApiEndpoint apiEndpoint);

        /// <summary>
        /// Удаляет эндпоинт у указанной сущности по имени
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="apiEntityName">Имя сущности с указанным эндпоинтом</param>
        /// <param name="id">Идентификатор изменяемого эндпоинта</param>
        Task DeleteAsync(string apiServiceName, string apiEntityName, string id);

        /// <summary>
        /// Изменяет активность у эндпоинта у указанной сущности
        /// </summary>
        /// <param name="status">True, если надо сделать активным эндпоинт, false - неактивным.</param>
        /// <param name="apiServiceName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="apiEntityName">Имя сущности с указанным эндпоинтом</param>
        /// <param name="id">Идентификатор изменяемого эндпоинта</param>
        Task ChangeActiveStatusAsync(string apiServiceName, string apiEntityName, string id, bool status);
    }
}
