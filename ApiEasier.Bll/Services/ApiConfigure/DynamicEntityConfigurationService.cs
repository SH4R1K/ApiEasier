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
        private readonly IDbResourceRepository _dbResourceRepository;
        private readonly IFileEntityRepository _fileEntityRepository;
        private readonly IConverter<ApiEntityDto, ApiEntity> _dtoToApiEntityConverter;

        public DynamicEntityConfigurationService(
            IFileEntityRepository fileEntityRepository,
            IDbResourceRepository dbResourceRepository,
            IConverter<ApiEntityDto, ApiEntity> dtoToApiEntityConverter)
        {
            _fileEntityRepository = fileEntityRepository;
            _dbResourceRepository = dbResourceRepository;
            _dtoToApiEntityConverter = dtoToApiEntityConverter;
        }

        public async Task<bool> DeleteAsync(string apiServiceName, string id)
        {
            var result = await _fileEntityRepository.DeleteAsync(apiServiceName, id);

            if (!result)
                return false;

            string resourceName = apiServiceName.Trim().Replace(" ", "") + "_" + id.Trim().Replace(" ", "");

            result = await _dbResourceRepository.DeleteAsync(resourceName);

            if (!result)
                return false;

            return true;
        }

        public async Task<bool> UpdateAsync(string apiServiceName, string entityName, ApiEntityDto entity)
        {
            var result = await _fileEntityRepository.UpdateAsync(apiServiceName, entityName, _dtoToApiEntityConverter.Convert(entity));

            return result;
        }

        public Task<bool> CreateAsync(string apiServiceName, ApiEntityDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
