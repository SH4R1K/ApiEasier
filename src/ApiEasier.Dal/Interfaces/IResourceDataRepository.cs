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

        /// <summary>
        /// Добавлет данные в ресурс хранения данных сущности
        /// </summary>
        /// <param name="resourceName">Имя ресурса хранения данных</param>
        /// <param name="data">Данные нового объекта сущности</param>
        /// <returns>Созданный объект сущности</returns>
        Task<DynamicResourceData> CreateDataAsync(string resourceName, object data);

        /// <summary>
        /// Изменяет данные объекта сущности в ресурсе хранения
        /// </summary>
        /// <param name="resourceName">Имя ресурса хранения данных</param>
        /// <param name="data">Данные нового объекта сущности</param>
        /// <param name="id">Идентификатор объекта сущности</param>
        /// <returns>Изменненый объект сущности</returns>
        Task<DynamicResourceData?> UpdateDataAsync(string resourceName, string id, object data);

        /// <summary>
        /// Удаляет объект указанной сущности по идентификатору
        /// </summary>
        /// <param name="resourceName">Имя ресурса хранения данных</param>
        /// <param name="id">Идентификатор объекта сущности</param>
        /// <returns>True, если удаление прошло успешно, false, если эндпоинт не был найден</returns>
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
