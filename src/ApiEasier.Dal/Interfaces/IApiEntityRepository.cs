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

        /// <summary>
        /// Возвращает сущность по идентификатору
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса, которому пренадлежит сущность</param>
        /// <param name="id">Идентификатор требуемой сущности</param>
        /// <returns>Требуемая сущность</returns>
        Task<ApiEntity?> GetByIdAsync(string apiServiceName, string id);

        /// <summary>
        /// Добавляет сущность к API-сервису
        /// </summary>
        /// <param name="apiServiceName">Имя изменяемого API-сервиса</param>
        /// <param name="apiEntity">Данные сущности для добавления</param>
        /// <returns>Добавленная сущность</returns>
        Task<ApiEntity?> CreateAsync(string apiServiceName, ApiEntity apiEntity);

        /// <summary>
        /// Изменяет сущность у API-сервиса
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса с изменяемой сущностью</param>
        /// <param name="id">Идентификатор изменяемой сущности</param>
        /// <param name="apiEntity">Новые данные сущности</param>
        /// <returns>True, если обновление прошло, false, если новое имя сущности уже существует</returns>
        Task<ApiEntity> UpdateAsync(string apiServiceName, string id, ApiEntity apiEntity);

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
