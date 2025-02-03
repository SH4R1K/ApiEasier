using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Dal.Interfaces.Db;
using ApiEasier.Dal.Interfaces.FileStorage;
using ApiEasier.Dm.Models;

namespace ApiEasier.Bll.Services.ApiConfigure
{
    class DynamicApiConfigurationService : IDynamicApiConfigurationService
    {
        private readonly IFileApiServiceRepository _fileApiServiceRepository;
        private readonly IDbResourceRepository _dbResourceRepository;
        private readonly IConverter<ApiService, ApiServiceDto> _apiServiceToDtoConverter;
        private readonly IConverter<ApiServiceDto, ApiService> _dtoToApiServiceConverter;

        public DynamicApiConfigurationService(
            IFileApiServiceRepository fileApiServiceRepository,
            IDbResourceRepository dbResourceRepository,
            IConverter<ApiService, ApiServiceDto> apiServiceToDtoConverter,
            IConverter<ApiServiceDto, ApiService> dtoToApiServiceConverter)
        {
            _fileApiServiceRepository = fileApiServiceRepository;
            _apiServiceToDtoConverter = apiServiceToDtoConverter;
            _dtoToApiServiceConverter = dtoToApiServiceConverter;
            _dbResourceRepository = dbResourceRepository;
        }

        public async Task<bool> CreateAsync(ApiServiceDto dto)
        {
            var apiService = _dtoToApiServiceConverter.Convert(dto);

            var result = await _fileApiServiceRepository.CreateAsync(apiService);
          
            return result;
        }

        public async Task<bool> Delete(string id)
        {
            var result = _fileApiServiceRepository.Delete(id);

            if (!result)
                return false;

            result = await _dbResourceRepository.DeleteAsync(id);

            return result;
        }

        public async Task<List<ApiServiceDto>> GetAsync()
        {
            var result = await _fileApiServiceRepository.GetAllAsync();
            return result.Select(_apiServiceToDtoConverter.Convert).ToList();
        }

        public async Task<ApiServiceDto?> GetByIdAsync(string id)
        {
            var result = await _fileApiServiceRepository.GetByIdAsync(id);

            if (result == null)
                return null;

            return _apiServiceToDtoConverter.Convert(result);
        }

        public async Task<bool> UpdateAsync(string id, ApiServiceDto dto)
        {
            var apiService = _dtoToApiServiceConverter.Convert(dto);

            var result = await _fileApiServiceRepository.UpdateAsync(id, apiService);
            return result;
        }
    }
}
