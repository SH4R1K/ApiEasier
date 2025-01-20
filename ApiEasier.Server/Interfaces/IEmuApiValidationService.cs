using ApiEasier.Server.Dto;
using ApiEasier.Server.Models;

namespace ApiEasier.Server.Interfaces
{
    /// <summary>
    /// Метод для валидации емулированных api-сервисов
    /// Тип возвращаемого значения кортеж - так как в будущем может понадобится 
    /// </summary>
    public interface IEmuApiValidationService
    {
        Task<(bool isValid, ApiServiceDto? apiService, ApiEntity? apiEntity)> ValidateApiAsync(
            string apiName,
            string entity,
            string endPoint,
            TypeResponse typeResponse);
    }
}
