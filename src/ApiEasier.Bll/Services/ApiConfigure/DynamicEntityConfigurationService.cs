using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Bll.Interfaces.Converter;
using ApiEasier.Dal.Interfaces;
using ApiEasier.Dm.Models;

namespace ApiEasier.Bll.Services.ApiConfigure
{
    /// <inheritdoc cref="IDynamicEntityConfigurationService"/>
    public class DynamicEntityConfigurationService : IDynamicEntityConfigurationService
    {
        private readonly IApiEntityRepository _apiEntityRepository;
        private readonly IConverter<ApiEntityDto, ApiEntity> _dtoToApiEntityConverter;
        private readonly IConverter<ApiEntity, ApiEntitySummaryDto> _apiEntityToDtoSummaryConverter;
        private readonly IConverter<ApiEntity, ApiEntityDto> _apiEntityToDtoConverter;

        public DynamicEntityConfigurationService(
            IApiEntityRepository apiEntityRepository,
            IConverter<ApiEntityDto, ApiEntity> dtoToApiEntityConverter,
            IConverter<ApiEntity, ApiEntitySummaryDto> apiEntityToDtoSummaryConverter,
            IConverter<ApiEntity, ApiEntityDto> apiEntityToDtoConverter)
        {
            _apiEntityRepository = apiEntityRepository;
            _dtoToApiEntityConverter = dtoToApiEntityConverter;
            _apiEntityToDtoSummaryConverter = apiEntityToDtoSummaryConverter;
            _apiEntityToDtoConverter = apiEntityToDtoConverter;
        }

        public async Task DeleteAsync(string apiServiceName, string id)
            => await _apiEntityRepository.DeleteAsync(apiServiceName, id);

        public async Task<ApiEntityDto> UpdateAsync(string apiServiceName, string id, ApiEntityDto entity)
        {
            var result = await _apiEntityRepository.UpdateAsync(apiServiceName, id, _dtoToApiEntityConverter.Convert(entity));

            return _apiEntityToDtoConverter.Convert(result);
        }

        public async Task<ApiEntityDto?> CreateAsync(string apiServiceName, ApiEntityDto entity)
        {
            var result = await _apiEntityRepository.CreateAsync(apiServiceName, _dtoToApiEntityConverter.Convert(entity));
            if (result == null)
                return null;

            return _apiEntityToDtoConverter.Convert(result);
        }

        public async Task ChangeActiveStatusAsync(string apiServiceName, string id, bool status)
            => await _apiEntityRepository.ChangeActiveStatusAsync(apiServiceName, id, status);

        public async Task<List<ApiEntitySummaryDto>?> GetAllAsync(string apiServiceName)
        {
            var result = await _apiEntityRepository.GetAllAsync(apiServiceName);
            if (result == null)
                return null;

            return result.Select(_apiEntityToDtoSummaryConverter.Convert).ToList();
        }

        public async Task<ApiEntityDto?> GetByIdAsync(string apiServiceName, string id)
        {
            var result = await _apiEntityRepository.GetByIdAsync(apiServiceName, id);
            if (result == null)
                return null;

            return _apiEntityToDtoConverter.Convert(result);
        }
    }
}
