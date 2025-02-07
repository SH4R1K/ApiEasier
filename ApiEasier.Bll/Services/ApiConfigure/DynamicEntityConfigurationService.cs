using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Bll.Interfaces.Converter;
using ApiEasier.Dal.Interfaces.FileStorage;
using ApiEasier.Dm.Models;

namespace ApiEasier.Bll.Services.ApiConfigure
{
    public class DynamicEntityConfigurationService : IDynamicEntityConfigurationService
    {
        private readonly IFileApiEntityRepository _fileApiEntityRepository;
        private readonly IConverter<ApiEntityDto, ApiEntity> _dtoToApiEntityConverter;
        private readonly IConverter<ApiEntity, ApiEntitySummaryDto> _apiEntityToDtoSummaryConverter;
        private readonly IConverter<ApiEntity, ApiEntityDto> _apiEntityToDtoConverter;

        public DynamicEntityConfigurationService(
            IFileApiEntityRepository fileApiEntityRepository,
            IConverter<ApiEntityDto, ApiEntity> dtoToApiEntityConverter,
            IConverter<ApiEntity, ApiEntitySummaryDto> apiEntityToDtoSummaryConverter,
            IConverter<ApiEntity, ApiEntityDto> apiEntityToDtoConverter)
        {
            _fileApiEntityRepository = fileApiEntityRepository;
            _dtoToApiEntityConverter = dtoToApiEntityConverter;
            _apiEntityToDtoSummaryConverter = apiEntityToDtoSummaryConverter;
            _apiEntityToDtoConverter = apiEntityToDtoConverter;
        }

        public async Task<bool> DeleteAsync(string apiServiceName, string id)
            => await _fileApiEntityRepository.DeleteAsync(apiServiceName, id);

        public async Task<bool> UpdateAsync(string apiServiceName, string entityName, ApiEntityDto entity)
            => await _fileApiEntityRepository.UpdateAsync(apiServiceName, entityName, _dtoToApiEntityConverter.Convert(entity));

        public async Task<bool> CreateAsync(string apiServiceName, ApiEntityDto entity)
            => await _fileApiEntityRepository.CreateAsync(apiServiceName, _dtoToApiEntityConverter.Convert(entity));

        public async Task<bool> ChangeActiveStatusAsync(string apiServiceName, string entityName, bool status)
            => await _fileApiEntityRepository.ChangeActiveStatusAsync(apiServiceName, entityName, status);

        public async Task<List<ApiEntitySummaryDto>> GetAsync(string apiServiceName)
        {
            var result = await _fileApiEntityRepository.GetAllAsync(apiServiceName);

            return result.Select(_apiEntityToDtoSummaryConverter.Convert).ToList();
        }

        public async Task<ApiEntityDto?> GetByIdAsync(string apiServiceName, string id)
        {
            var result = await _fileApiEntityRepository.GetByIdAsync(apiServiceName, id);
            if (result == null)
                return null;

            return _apiEntityToDtoConverter.Convert(result);
        }
    }
}
