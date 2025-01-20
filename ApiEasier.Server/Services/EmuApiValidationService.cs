using ApiEasier.Server.Dto;
using ApiEasier.Server.Interfaces;
using ApiEasier.Server.Models;
using MongoDB.Bson;
using NJsonSchema;
using System.Text.Json;

namespace ApiEasier.Server.Services
{
    public class EmuApiValidationService : IEmuApiValidationService
    {
        private readonly JsonService _jsonService;

        public EmuApiValidationService(JsonService jsonService)
        {
            _jsonService = jsonService;
        }

        public async Task<(bool isValid, ApiServiceDto? apiService, ApiEntity? apiEntity)> ValidateApiAsync(
            string apiName,
            string entityName,
            string endpoint,
            TypeResponse typeResponse)
        {
            // Проверяем существование api-сервиса в json-файле конфигурации
            var api = await _jsonService.GetApiServiceByNameAsync(apiName);
            if (api == null)
                return (false, null, null);

            // Проверяем наличие сущности у api-сервиса в json-файле конфигурации
            var entity = api.Entities.FirstOrDefault(e => e.Name == entityName);
            if (entity == null)
                return (false, api, null);

            // Проверяем маршрут и тип ответа у api-сервиса в json-файле конфигурации
            var isEndpointValid = entity.Actions.Any(a => a.IsActive && a.Route == endpoint && a.Type == typeResponse);
            if (!isEndpointValid)
                return (false, api, entity);

            return (true, api, entity);
        }

        public async Task<bool> ValidateEntityStructureAsync(ApiEntity apiEntity, object document)
        {
            var json = JsonSerializer.Serialize(document);
            var jsonSchema = await JsonSchema.FromJsonAsync(JsonSerializer.Serialize(apiEntity.Structure));
            return jsonSchema.Validate(json).Count == 0;
        }
    }
}
