﻿using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Bll.Interfaces.Converter;
using ApiEasier.Dal.Interfaces;
using ApiEasier.Dm.Models;

namespace ApiEasier.Bll.Services.ApiConfigure
{
    /// <inheritdoc cref="IDynamicApiConfigurationService"/>
    public class DynamicApiConfigurationService : IDynamicApiConfigurationService
    {

        private readonly IApiServiceRepository _apiServiceRepository;
        private readonly IResourceRepository _dbResourceRepository;
        
        private readonly IConverter<ApiService, ApiServiceDto> _apiServiceToDtoConverter;
        private readonly IConverter<ApiServiceDto, ApiService> _dtoToApiServiceConverter;
        private readonly IConverter<ApiService, ApiServiceSummaryDto> _apiServiceToDtoSummaryConverter;

        public DynamicApiConfigurationService(
            IApiServiceRepository apiServiceRepository,
            IResourceRepository dbResourceRepository,
            IConverter<ApiService, ApiServiceDto> apiServiceToDtoConverter,
            IConverter<ApiServiceDto, ApiService> dtoToApiServiceConverter,
            IConverter<ApiService, ApiServiceSummaryDto> apiServiceToDtoSummaryConverter)
        {
            _apiServiceRepository = apiServiceRepository;
            _dbResourceRepository = dbResourceRepository;
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

        public async Task<ApiServiceDto?> CreateAsync(ApiServiceDto apiServiceDto)
        {
            var apiService = _dtoToApiServiceConverter.Convert(apiServiceDto);

            var result = await _apiServiceRepository.CreateAsync(apiService);
            if (result == null)
                return null;

            return _apiServiceToDtoConverter.Convert(result!);
        }

        /// <summary>
        /// Удаление файла конфигурации api-сервиса и ресурсов в бд с ним связанных
        /// </summary>
        /// <param name="id">название api-сервиса</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string id)
        {
            if (await _apiServiceRepository.GetByIdAsync(id) == null)
                return false;

            var result = await _apiServiceRepository.DeleteAsync(id);

            await _dbResourceRepository.DeleteByApiNameAsync(id);

            return result;
        }

        public async Task<List<ApiServiceSummaryDto>> GetAllAsync()
        {
            var result = await _apiServiceRepository.GetAllAsync();
            return result.Select(_apiServiceToDtoSummaryConverter.Convert).ToList();
        }

        public async Task<ApiServiceDto?> GetByIdAsync(string id)
        {
            var result = await _apiServiceRepository.GetByIdAsync(id);

            if (result == null)
                return null;

            return _apiServiceToDtoConverter.Convert(result);
        }

        public async Task<ApiServiceDto?> UpdateAsync(string id, ApiServiceDto apiServiceDto)
        {
            var apiService = _dtoToApiServiceConverter.Convert(apiServiceDto);

            var result = await _apiServiceRepository.UpdateAsync(id, apiService);

            if (result == null)
                return null;

            if (id != apiServiceDto.Name)
            {
                await _dbResourceRepository.UpdateByApiNameAsync(id, apiServiceDto.Name);
            }

            return _apiServiceToDtoConverter.Convert(result);
        }
    }
}
