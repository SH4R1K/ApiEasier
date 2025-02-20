using ApiEasier.Dm.Models;

namespace ApiEasier.Bll.Interfaces.ApiEmu
{
    /// <summary>
    /// Позволяет управлять данными сущностей
    /// </summary>
    public interface IDynamicResourceDataService
    {
        /// <summary>
        /// Возвращает все или отфильтрованные данные, принадлежащие указанной сущности
        /// </summary>
        /// <param name="apiName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="apiEntityName">Имя сущности, имеющей эти данные</param>
        /// <param name="endpoint">Эндпоинт сущности с типом Get</param>
        /// <param name="filters">Фильтры данных</param>
        /// <returns>Список данных сущности</returns>
        public Task<List<DynamicResourceData>?> GetAsync(string apiName, string apiEntityName, string endpoint, string? filters);

        /// <summary>
        /// Возвращает данные объекта указанной сущности по идентификатору
        /// </summary>
        /// <param name="apiName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="apiEntityName">Имя сущности, имеющей эти данные</param>
        /// <param name="endpoint">Эндпоинт сущности с типом Get</param>
        /// <param name="id">Идентификатор данных</param>
        /// <returns>Объект указанной сущности</returns>
        public Task<DynamicResourceData?> GetByIdAsync(string apiName, string apiEntityName, string endpoint, string id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="apiEntityName"></param>
        /// <param name="endpoint"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public Task<DynamicResourceData?> AddAsync(string apiName, string apiEntityName, string endpoint, object json);
        public Task<DynamicResourceData?> UpdateAsync(string apiName, string apiEntityName, string endpoint, string id, object json);
        public Task<bool> Delete(string apiName, string apiEntityName, string endpoint, string id);
    }
}
