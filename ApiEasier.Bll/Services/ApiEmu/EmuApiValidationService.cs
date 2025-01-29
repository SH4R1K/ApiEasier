using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Bll.Interfaces.ApiEmu;
using ApiEasier.Server.Models;
using NJsonSchema;
using System.Text.Json;

namespace ApiEasier.Bll.Services.ApiEmu
{
    /// <summary>
    /// Сервис для валидации API и его сущностей.
    /// </summary>
    public class EmuApiValidationService : IEmuApiValidationService
    {
        private readonly IConfigFileApiService _configFileApiService;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EmuApiValidationService"/>.
        /// </summary>
        /// <param name="configFileApiService">Сервис для работы с конфигурационными файлами API.</param>
        public EmuApiValidationService(IConfigFileApiService configFileApiService)
        {
            _configFileApiService = configFileApiService;
        }

        /// <summary>
        /// Проверяет валидность API, сущности и конечной точки.
        /// </summary>
        /// <param name="apiName">Имя API.</param>
        /// <param name="entityName">Имя сущности.</param>
        /// <param name="endpoint">Конечная точка.</param>
        /// <param name="typeResponse">Тип ответа.</param>
        /// <returns>Кортеж, содержащий информацию о валидности, API-сервисе и сущности.</returns>
        /// <exception cref="InvalidOperationException">Выбрасывается, если произошла ошибка при валидации API.</exception>
        public async Task<(bool isValid, ApiServiceDto? apiService, ApiEntity? apiEntity)> ValidateApiAsync(
            string apiName,
            string entityName,
            string endpoint,
            TypeResponse typeResponse)
        {
            try
            {
                // Проверяем существование api-сервиса в json-файле конфигурации
                var api = await _configFileApiService.GetApiServiceByNameAsync(apiName);
                if (api == null || !api.IsActive)
                    return (false, null, null);

                // Проверяем наличие сущности у api-сервиса в json-файле конфигурации
                var entity = api.Entities.FirstOrDefault(e => e.Name == entityName && e.IsActive);
                if (entity == null)
                    return (false, api, null);

                // Проверяем маршрут и тип ответа у api-сервиса в json-файле конфигурации
                var isEndpointValid = entity.Actions.Any(a => a.IsActive && a.Route == endpoint && a.Type == typeResponse);
                if (!isEndpointValid)
                    return (false, api, entity);

                return (true, api, entity);
            }
            catch (Exception ex)
            {
                // Логирование или обработка исключения
                throw new InvalidOperationException("Ошибка при валидации API.", ex);
            }
        }

        /// <summary>
        /// Проверяет структуру сущности на соответствие заданной схеме.
        /// </summary>
        /// <param name="apiEntity">Сущность API.</param>
        /// <param name="document">Документ для проверки.</param>
        /// <returns>True, если структура документа соответствует схеме; иначе false.</returns>
        /// <exception cref="InvalidOperationException">Выбрасывается, если произошла ошибка при валидации структуры сущности.</exception>
        public async Task<bool> ValidateEntityStructureAsync(ApiEntity apiEntity, object document)
        {
            try
            {
                if (apiEntity.Structure == null)
                    return true;

                var json = JsonSerializer.Serialize(document);
                var jsonSchema = await JsonSchema.FromJsonAsync(JsonSerializer.Serialize(apiEntity.Structure));
                return jsonSchema.Validate(json).Count == 0;
            }
            catch (Exception ex)
            {
                // Логирование или обработка исключения
                throw new InvalidOperationException("Ошибка при валидации структуры сущности.", ex);
            }
        }
    }
}
