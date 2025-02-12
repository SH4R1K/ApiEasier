using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Bll.Interfaces.Converter;
using ApiEasier.Dal.Interfaces.Db;
using ApiEasier.Dal.Interfaces.FileStorage;
using ApiEasier.Dm.Models;

namespace ApiEasier.Bll.Services.ApiConfigure
{
    public class DynamicEntityConfigurationService : IDynamicEntityConfigurationService
    {
        private readonly IFileApiEntityRepository _fileApiEntityRepository;
        private readonly IDbResourceRepository _dbResourceRepository;

        private readonly IConverter<ApiEntityDto, ApiEntity> _dtoToApiEntityConverter;
        private readonly IConverter<ApiEntity, ApiEntitySummaryDto> _apiEntityToDtoSummaryConverter;
        private readonly IConverter<ApiEntity, ApiEntityDto> _apiEntityToDtoConverter;

        public DynamicEntityConfigurationService(
            IFileApiEntityRepository fileApiEntityRepository,
            IDbResourceRepository dbResourceRepository,
            IConverter<ApiEntityDto, ApiEntity> dtoToApiEntityConverter,
            IConverter<ApiEntity, ApiEntitySummaryDto> apiEntityToDtoSummaryConverter,
            IConverter<ApiEntity, ApiEntityDto> apiEntityToDtoConverter)
        {
            _fileApiEntityRepository = fileApiEntityRepository;
            _dbResourceRepository = dbResourceRepository;

            _dtoToApiEntityConverter = dtoToApiEntityConverter;
            _apiEntityToDtoSummaryConverter = apiEntityToDtoSummaryConverter;
            _apiEntityToDtoConverter = apiEntityToDtoConverter;
        }

        public async Task<bool> DeleteAsync(string apiServiceName, string id)
        {
            await _fileApiEntityRepository.DeleteAsync(apiServiceName, id);
            await _dbResourceRepository.DeleteByApiEntityNameAsync(id);

            return true;
        }

        public async Task<bool> UpdateAsync(string apiServiceName, string entityName, ApiEntityDto apiEntity)
        {
            // ошибка если коллекции нет
            if (entityName != apiEntity.Name)
            {
                await _dbResourceRepository.UpdateByApiEntityNameAsync(apiServiceName, entityName, apiEntity.Name);
            }

            return await _fileApiEntityRepository.UpdateAsync(apiServiceName, entityName, _dtoToApiEntityConverter.Convert(apiEntity));
        }

        public async Task<ApiEntityDto?> CreateAsync(string apiServiceName, ApiEntityDto entity)
        {
            var result = await _fileApiEntityRepository.CreateAsync(apiServiceName, _dtoToApiEntityConverter.Convert(entity));
            if (result == null)
                return null;

            return _apiEntityToDtoConverter.Convert(result);
        }
            
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
