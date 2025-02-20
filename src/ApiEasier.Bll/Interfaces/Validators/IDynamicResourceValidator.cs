using ApiEasier.Bll.Dto;

namespace ApiEasier.Bll.Interfaces.Validators
{
    /// <summary>
    /// Интерфейс для валидации эмулированных API-сервисов.
    /// </summary>
    public interface IDynamicResourceValidator
    {
        /// <summary>
        /// Проверяет существование эндпоинта по адресу API-сервис/сущность/эндпоинт.
        /// </summary>
        /// <param name="apiName">Имя API-сервиса.</param>
        /// <param name="entity">Имя сущности, связанной с API-сервисом.</param>
        /// <param name="endpoint">Эндпоинт, связанный с сущностью.</param>
        /// <param name="typeResponse">Ожидаемый тип эндпоинта.</param>
        /// <returns>
        /// Кортеж, содержащий:
        /// <list type="bullet">
        /// <item><description><c>isValid</c> - указывает, является ли API-сервис валидным.</description></item>
        /// <item><description><c>apiService</c> - API-сервис, если он валиден.</description></item>
        /// <item><description><c>apiEntity</c> - объект, представляющий сущность API, если она валидна.</description></item>
        /// </list>
        /// </returns>
        Task<(bool isValid, ApiServiceDto? apiService, ApiEntityDto? apiEntity)> ValidateApiAsync(
            string apiName,
            string entity,
            string endpoint,
            string typeResponse);

        /// <summary>
        /// Валидирует структуру объекта на основе предоставленного структуры данных сущности.
        /// </summary>
        /// <param name="apiEntity">Данные сущности со структурой данных.</param>
        /// <param name="document">Объект сущности, структура которого будет проверяться.</param>
        /// <returns>Возвращает <c>true</c>, если структура документа соответствует сущности, иначе <c>false</c>.</returns>
        Task<bool> ValidateEntityStructureAsync(ApiEntityDto apiEntity, object document);
    }
}
