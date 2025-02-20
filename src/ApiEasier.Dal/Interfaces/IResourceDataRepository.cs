using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Interfaces
{
    /// <summary>
    /// Позволяет управлять данными сущности
    /// </summary>
    public interface IResourceDataRepository
    {
        /// <summary>
        /// Возвращает все или отфильтрованные данные сущности
        /// </summary>
        /// <param name="resourceName">Имя ресурса хранения данных</param>
        /// <param name="filter">Фильтры данных</param>
        /// <returns>Данные сущности</returns>
        Task<List<DynamicResourceData>?> GetAllDataAsync(string resourceName, string? filter);
        Task<DynamicResourceData> CreateDataAsync(string resourceName, object data);
        Task<DynamicResourceData?> UpdateDataAsync(string resourceName, string id, object data);
        Task<bool> DeleteDataAsync(string resourceName, string id);

        /// <summary>
        /// Возвращает данные объекта указанной сущности по идентификатору
        /// </summary>
        /// <param name="resourceName">Имя ресурса хранения данных</param>
        /// <param name="id">Идентификатор объекта сущности</param>
        /// <returns>Объект сущности</returns>
        Task<DynamicResourceData?> GetDataByIdAsync(string resourceName, string id);
    }
}
