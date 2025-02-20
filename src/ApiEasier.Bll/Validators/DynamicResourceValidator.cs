using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.Converter;
using ApiEasier.Bll.Interfaces.Validators;
using ApiEasier.Dal.Interfaces;
using ApiEasier.Dm.Models;
using NJsonSchema;
using System.Text.Json;

namespace ApiEasier.Bll.Validators
{

    public class DynamicResourceValidator(
        IApiServiceRepository fileApiServiceRepository,
        IConverter<ApiService, ApiServiceDto> apiServiceToDtoConverter,
        IConverter<ApiEntity, ApiEntityDto> apiEntityToDtoConverter) : IDynamicResourceValidator
    {
        private readonly IApiServiceRepository _fileApiServiceRepository = fileApiServiceRepository;
        private readonly IConverter<ApiService, ApiServiceDto> _apiServiceToDtoConverter = apiServiceToDtoConverter;
        private readonly IConverter<ApiEntity, ApiEntityDto> _apiEntityToDtoConverter = apiEntityToDtoConverter;

        public async Task<(bool isValid, ApiServiceDto? apiService, ApiEntityDto? apiEntity)> ValidateApiAsync(
            string apiName,
            string entityName,
            string endpoint,
            string typeResponse)
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

        public async Task<bool> ValidateEntityStructureAsync(ApiEntityDto apiEntity, object document)
        {
            if (apiEntity.Structure == null)
                return true;

            var json = JsonSerializer.Serialize(document);
            var jsonSchema = await JsonSchema.FromJsonAsync(JsonSerializer.Serialize(apiEntity.Structure));
            return jsonSchema.Validate(json).Count == 0;
        }
    }
}
