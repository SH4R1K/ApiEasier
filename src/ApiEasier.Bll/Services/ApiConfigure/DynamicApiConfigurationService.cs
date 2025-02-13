using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Bll.Interfaces.Converter;
using ApiEasier.Dal.Interfaces.FileStorage;
using ApiEasier.Dm.Models;

namespace ApiEasier.Bll.Services.ApiConfigure
{
    /// <summary>
    /// Обеспечивает настройку API-сервисов
    /// </summary>
    public class DynamicApiConfigurationService : IDynamicApiConfigurationService
    {
        private readonly IApiServiceRepository _apiServiceRepository;
        private readonly IConverter<ApiService, ApiServiceDto> _apiServiceToDtoConverter;
        private readonly IConverter<ApiServiceDto, ApiService> _dtoToApiServiceConverter;
        private readonly IConverter<ApiService, ApiServiceSummaryDto> _apiServiceToDtoSummaryConverter;

        public DynamicApiConfigurationService(
            IApiServiceRepository apiServiceRepository,
            IConverter<ApiService, ApiServiceDto> apiServiceToDtoConverter,
            IConverter<ApiServiceDto, ApiService> dtoToApiServiceConverter,
            IConverter<ApiService, ApiServiceSummaryDto> apiServiceToDtoSummaryConverter)
        {
            _apiServiceRepository = apiServiceRepository;
            _apiServiceToDtoConverter = apiServiceToDtoConverter;
            _dtoToApiServiceConverter = dtoToApiServiceConverter;
            _apiServiceToDtoSummaryConverter = apiServiceToDtoSummaryConverter;
        }

        /// <summary>
        /// Изменение статуса активности api-сервиса в его файле конфигурации
        /// </summary>
        /// <param name="id">название api-сервиса</param>
        /// <param name="status">новый статус активности</param>
        /// <returns></returns>
        public async Task<bool> ChangeActiveStatusAsync(string id, bool status)
        {
            return await _apiServiceRepository.ChangeActiveStatusAsync(id, status);
        }

        /// <summary>
        /// Создание файла конфигурации api-сервиса 
        /// </summary>
        /// <param name="dto">данные для создания</param>
        /// <returns></returns>
        public async Task<ApiServiceDto?> CreateAsync(ApiServiceDto dto)
        {
            var apiService = _dtoToApiServiceConverter.Convert(dto);

            var result = await _apiServiceRepository.CreateAsync(apiService);
            if (result == null)
                return null;

            return _apiServiceToDtoConverter.Convert(result);
        }

        /// <summary>
        /// Удаление файла конфигурации api-сервиса
        /// </summary>
        /// <param name="id">название api-сервиса</param>
        /// <returns></returns>
        public bool Delete(string id)
        {
            var result = _apiServiceRepository.Delete(id);
            if (!result)
                return false;

            return result;
        }

        public async Task<List<ApiServiceSummaryDto>> GetAllAsync()
        {
            var result = await _apiServiceRepository.GetAllAsync();
            return result.Select(_apiServiceToDtoSummaryConverter.Convert).ToList();
        }

        /// <summary>
        /// Вывод api-сервиса по названию
        /// </summary>
        /// <param name="id">название файла конфигурации</param>
        /// <returns></returns>
        public async Task<ApiServiceDto?> GetByIdAsync(string id)
        {
            var result = await _apiServiceRepository.GetByIdAsync(id);

            if (result == null)
                return null;

            return _apiServiceToDtoConverter.Convert(result);
        }

        /// <summary>
        /// Обновление данных у api-сервиса в файле конфигурации
        /// </summary>
        /// <param name="id">название файла</param>
        /// <param name="dto">данные</param>
        /// <returns></returns>
        public async Task<ApiServiceDto?> UpdateAsync(string id, ApiServiceDto dto)
        {
            var apiService = _dtoToApiServiceConverter.Convert(dto);

            var result = await _apiServiceRepository.UpdateAsync(id, apiService);

            if (result == null)
                return default;

            return _apiServiceToDtoConverter.Convert(result);
        }
    }
}
