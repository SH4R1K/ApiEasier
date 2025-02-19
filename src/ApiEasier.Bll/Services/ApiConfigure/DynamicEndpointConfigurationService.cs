using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Bll.Interfaces.Converter;
using ApiEasier.Dal.Interfaces;
using ApiEasier.Dm.Models;

namespace ApiEasier.Bll.Services.ApiConfigure
{
    public class DynamicEndpointConfigurationService : IDynamicEndpointConfigurationService
    {
        private readonly IApiEndpointRepository _apiEndpointRepository;
        private readonly IConverter<ApiEndpointDto, ApiEndpoint> _dtoToApiEndpointConverter;
        private readonly IConverter<ApiEndpoint, ApiEndpointDto> _apiEndpointToDtoConverter;

        public DynamicEndpointConfigurationService(
            IApiEndpointRepository apiEndpointRepository,
            IConverter<ApiEndpointDto, ApiEndpoint> dtoToApiEndpointConverter,
            IConverter<ApiEndpoint, ApiEndpointDto> apiEndpointToDtoConverter)
        {
            _apiEndpointRepository = apiEndpointRepository;
            _dtoToApiEndpointConverter = dtoToApiEndpointConverter;
            _apiEndpointToDtoConverter = apiEndpointToDtoConverter;
        }

        public async Task<bool> ChangeActiveStatusAsync(string apiServiceName, string entityName, string endpointName, bool status)
            => await _apiEndpointRepository.ChangeActiveStatusAsync(apiServiceName, entityName, endpointName, status);

        public async Task<ApiEndpointDto?> CreateAsync(string apiServiceName, string apiEntityName, ApiEndpointDto apiEndpointDto)
        {
            var result = await _apiEndpointRepository.CreateAsync(
                apiServiceName,
                apiEntityName,
                _dtoToApiEndpointConverter.Convert(apiEndpointDto)
            );

            if (result == null )
                return null;

            return _apiEndpointToDtoConverter.Convert(result);
        }
        
        public async Task<bool> DeleteAsync(string apiServiceName, string apiEntityName, string id)
            => await _apiEndpointRepository.DeleteAsync(apiServiceName, apiEntityName, id);

        public async Task<bool> UpdateAsync(string apiServiceName, string apiEntityName, string apiEndpointName, ApiEndpointDto apiEndpointDto)
            => await _apiEndpointRepository.UpdateAsync(
                apiServiceName,
                apiEntityName,
                apiEndpointName,
                _dtoToApiEndpointConverter.Convert(apiEndpointDto)
            );

        public async Task<List<ApiEndpointDto>> GetAsync(string apiServiceName, string apiEntityName)
        {
            var result = await _apiEndpointRepository.GetAllAsync(apiServiceName, apiEntityName);

            return result.Select(_apiEndpointToDtoConverter.Convert).ToList();
        }

        public async Task<ApiEndpointDto?> GetByIdAsync(string apiServiceName, string apiEntityName, string id)
        {
            var result = await _apiEndpointRepository.GetByIdAsync(apiServiceName, apiEntityName, id);
            if (result == null)
                return null;

            return _apiEndpointToDtoConverter.Convert(result);
        }
    }
}
