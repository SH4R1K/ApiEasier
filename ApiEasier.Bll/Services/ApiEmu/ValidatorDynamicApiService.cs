using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiEmu;
using ApiEasier.Bll.Interfaces.Converter;
using ApiEasier.Dal.Interfaces.FileStorage;
using ApiEasier.Dm.Models;
using NJsonSchema;
using System.Text.Json;

namespace ApiEasier.Bll.Services.ApiEmu
{

    public class ValidatorDynamicApiService(
        IFileApiServiceRepository fileApiServiceRepository,
        IConverter<ApiService, ApiServiceDto> apiServiceToDtoConverter,
        IConverter<ApiEntity, ApiEntityDto> apiEntityToDtoConverter) : IValidatorDynamicApiService
    {
        private readonly IFileApiServiceRepository _fileApiServiceRepository = fileApiServiceRepository;
        private readonly IConverter<ApiService, ApiServiceDto> _apiServiceToDtoConverter = apiServiceToDtoConverter;
        private readonly IConverter<ApiEntity, ApiEntityDto> _apiEntityToDtoConverter = apiEntityToDtoConverter;

        /// <summary>
        /// Проверяет валидность API, сущности и конечной точки.
        /// </summary>
        /// <param name="apiName">Имя API.</param>
        /// <param name="entityName">Имя сущности.</param>
        /// <param name="endpoint">Конечная точка.</param>
        /// <param name="typeResponse">Тип ответа.</param>
        /// <returns>Кортеж, содержащий информацию о валидности, API-сервисе и сущности.</returns>
        /// <exception cref="InvalidOperationException">Выбрасывается, если произошла ошибка при валидации API.</exception>
        public async Task<(bool isValid, ApiServiceDto? apiService, ApiEntityDto? apiEntity)> ValidateApiAsync(
            string apiName,
            string entityName,
            string endpoint,
            string typeResponse)
        {
            try
            {
                // Проверяем существование api-сервиса в json-файле конфигурации
                var api = await _fileApiServiceRepository.GetByIdAsync(apiName);
                if (api == null || !api.IsActive)
                    return (false, null, null);

                // Проверяем наличие сущности у api-сервиса в json-файле конфигурации
                var entity = api.Entities.FirstOrDefault(e => e.Name == entityName && e.IsActive);
                if (entity == null)
                    return (false, _apiServiceToDtoConverter.Convert(api), null);

                // Проверяем маршрут и тип ответа у api-сервиса в json-файле конфигурации
                var isEndpointValid = entity.Endpoints.Any(a => a.IsActive && a.Route == endpoint && a.Type.ToString().ToLower() == typeResponse.ToLower());
                if (!isEndpointValid)
                    return (false, _apiServiceToDtoConverter.Convert(api), _apiEntityToDtoConverter.Convert(entity));

                return (true, _apiServiceToDtoConverter.Convert(api), _apiEntityToDtoConverter.Convert(entity));
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
        public async Task<bool> ValidateEntityStructureAsync(ApiEntityDto apiEntity, object document)
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
