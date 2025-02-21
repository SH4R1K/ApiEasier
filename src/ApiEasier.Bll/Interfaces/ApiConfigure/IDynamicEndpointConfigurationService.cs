using ApiEasier.Bll.Dto;

namespace ApiEasier.Bll.Interfaces.ApiConfigure
{
    /// <summary>
    /// Позволяет управлять эндпоинтами сущности
    /// </summary>
    public interface IDynamicEndpointConfigurationService
    {
        /// <summary>
        /// Возвращет все эндпоинты указанной сущности
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="apiEntityName">Имя сущности с требуемыми эндпоинтами</param>
        /// <returns>Список эндпоинтов указанной сущности</returns>
        Task<List<ApiEndpointDto>> GetAllAsync(string apiServiceName, string apiEntityName);

        /// <summary>
        /// Возвращает эндпоинт указанной сущности по имени
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="apiEntityName">Имя сущности с указанным эндпоинтом</param>
        /// <param name="id">Идентификатор требуемого эндпоинта</param>
        /// <returns>Требуемый эндпоинт</returns>
        Task<ApiEndpointDto?> GetByIdAsync(string apiServiceName, string apiEntityName, string id);

        /// <summary>
        /// Создает эндпоинт в указанной сущности
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="apiEntityName">Имя сущности с указанным эндпоинтом</param>
        /// <param name="apiEndpointDto">Данные нового эндпоинта</param>
        /// <returns>Созданнный эндпоинт</returns>
        Task<ApiEndpointDto?> CreateAsync(string apiServiceName, string apiEntityName, ApiEndpointDto apiEndpointDto);

        /// <summary>
        /// Изменяет эндпоинт у указанной сущности
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="apiEntityName">Имя сущности с указанным эндпоинтом</param>
        /// <param name="id">Идентификатор изменяемого эндпоинта</param>
        /// <param name="apiEndpointDto">Новые данные эндпоинта</param>
        /// <returns>Изменненый эндпоинт</returns>
        Task<ApiEndpointDto> UpdateAsync(string apiServiceName, string apiEntityName, string id, ApiEndpointDto apiEndpointDto);

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
        /// <param name="entityName">Имя сущности с указанным эндпоинтом</param>
        /// <param name="id">Идентификатор изменяемого эндпоинта</param>
        Task ChangeActiveStatusAsync(string apiServiceName, string entityName, string id, bool status);
    }
}
