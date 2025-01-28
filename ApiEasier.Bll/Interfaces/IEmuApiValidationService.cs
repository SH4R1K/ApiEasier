using ApiEasier.Bll.Dto;
using ApiEasier.Server.Models;

namespace ApiEasier.Bll.Interfaces
{
    /// <summary>
    /// Интерфейс для валидации эмулированных API-сервисов.
    /// </summary>
    public interface IEmuApiValidationService
    {
        /// <summary>
        /// Асинхронно валидирует API-сервис по заданным параметрам.
        /// </summary>
        /// <param name="apiName">Имя API-сервиса.</param>
        /// <param name="entity">Имя сущности, связанной с API.</param>
        /// <param name="endPoint">Конечная точка API для валидации.</param>
        /// <param name="typeResponse">Тип ожидаемого ответа от API.</param>
        /// <returns>
        /// Кортеж, содержащий:
        /// <list type="bullet">
        /// <item><description><c>isValid</c> - указывает, является ли API-сервис валидным.</description></item>
        /// <item><description><c>apiService</c> - API-сервис, если он валиден.</description></item>
        /// <item><description><c>apiEntity</c> - объект, представляющий сущность API, если она валидна.</description></item>
        /// </list>
        /// </returns>
        Task<(bool isValid, ApiServiceDto? apiService, ApiEntity? apiEntity)> ValidateApiAsync(
            string apiName,
            string entity,
            string endPoint,
            TypeResponse typeResponse);

        /// <summary>
        /// Асинхронно валидирует структуру сущности на основе предоставленного объекта сущности.
        /// </summary>
        /// <param name="apiEntity">Сущность API для валидации.</param>
        /// <param name="document">Объект сущности, структура которого будет проверяться.</param>
        /// <returns>Возвращает <c>true</c>, если структура документа соответствует сущности, иначе <c>false</c>.</returns>
        Task<bool> ValidateEntityStructureAsync(ApiEntity apiEntity, object document);
    }
}
