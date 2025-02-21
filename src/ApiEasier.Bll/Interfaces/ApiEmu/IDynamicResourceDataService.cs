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
        /// <param name="endpoint">Эндпоинт сущности с типом GetByIndex</param>
        /// <param name="id">Идентификатор данных</param>
        /// <returns>Объект указанной сущности</returns>
        public Task<DynamicResourceData?> GetByIdAsync(string apiName, string apiEntityName, string endpoint, string id);

        /// <summary>
        /// Создает объект указаной сущности 
        /// </summary>
        /// <param name="apiName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="apiEntityName">Имя сущности, имеющей эти данные</param>
        /// <param name="endpoint">Эндпоинт сущности с типом Post</param>
        /// <param name="data">Данные нового объекта</param>
        /// <returns>Созданный объект сущности</returns>
        public Task<DynamicResourceData?> AddAsync(string apiName, string apiEntityName, string endpoint, object data);

        /// <summary>
        /// Изменяет объект указанной сущности по идентификатору
        /// </summary>
        /// <param name="apiName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="apiEntityName">Имя сущности, имеющей эти данные</param>
        /// <param name="endpoint">Эндпоинт сущности с типом Put</param>
        /// <param name="id">Идентификатор объекта</param>
        /// <param name="data">Данные нового объекта</param>
        /// <returns>Изменненый объект сущности</returns>
        public Task<DynamicResourceData?> UpdateAsync(string apiName, string apiEntityName, string endpoint, string id, object data);

        /// <summary>
        /// Удаляет объект указанной сущности по идентификатору
        /// </summary>
        /// <param name="apiName">Имя API-сервиса с указанной сущностью</param>
        /// <param name="apiEntityName">Имя сущности, имеющей эти данные</param>
        /// <param name="endpoint">Эндпоинт сущности с типом Put</param>
        /// <param name="id">Идентификатор объекта сущности</param>
        /// <returns>True, если удаление прошло успешно, false, если эндпоинт не был найден</returns>
        public Task<bool> Delete(string apiName, string apiEntityName, string endpoint, string id);
    }
}
