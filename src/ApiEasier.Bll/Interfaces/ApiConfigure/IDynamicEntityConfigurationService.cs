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

        /// <summary>
        /// Возвращает сущность по идентификатору
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса, которому пренадлежит сущность</param>
        /// <param name="id">Идентификатор требуемой сущности</param>
        /// <returns>Требуемой сущности</returns>
        Task<ApiEntityDto?> GetByIdAsync(string apiServiceName, string id);

        /// <summary>
        /// Добавляет сущность к API-сервису
        /// </summary>
        /// <param name="apiServiceName">Имя изменяемого API-сервиса</param>
        /// <param name="entity">Данные сущности для добавления</param>
        /// <returns>Добавленная сущность</returns>
        Task<ApiEntityDto?> CreateAsync(string apiServiceName, ApiEntityDto entity);

        /// <summary>
        /// Изменяет сущность у API-сервиса
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса с изменяемой сущностью</param>
        /// <param name="id">Идентификатор изменяемой сущности</param>
        /// <param name="entity">Новые данные сущности</param>
        /// <returns>True, если обновление прошло, false, если новое имя сущности уже существует</returns>
        Task<ApiEntityDto> UpdateAsync(string apiServiceName, string id, ApiEntityDto entity);

        /// <summary>
        /// Удаляет сущность внутри API-сервиса
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса с удаляемой сущностью</param>
        /// <param name="id">Идентификатор удаляемой сущности</param>
        /// <returns>True, если сущность удалена успешно, false, если сущность не была найдена</returns>
        Task DeleteAsync(string apiServiceName, string id);

        /// <summary>
        /// Изменяет активность сущности у API-сервиса 
        /// </summary>
        /// <param name="status">True, если надо сделать сущность активной, false - неактивной</param>
        /// <param name="apiServiceName">Имя API-сервиса с изменяеммой сущностью</param>
        /// <param name="id">Идентификатор изменяемой сущности</param>
        Task ChangeActiveStatusAsync(string apiServiceName, string id, bool status);
    }
}
