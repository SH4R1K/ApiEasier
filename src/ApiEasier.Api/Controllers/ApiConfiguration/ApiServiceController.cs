﻿using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Dal.Exceptions;
using ApiEasier.Logger.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApiEasier.Api.Controllers.ApiConfiguration
{

    /// <summary>
    /// Позволяет настраивать API-сервисы 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApiServiceController(
        IDynamicApiConfigurationService dynamicApiConfigurationService,
        ILoggerService logger) : ControllerBase
    {
        private readonly IDynamicApiConfigurationService _dynamicApiConfigurationService = dynamicApiConfigurationService;
        private readonly ILoggerService _logger = logger;

        /// <summary>
        /// Возвращает список всех API-сервисов без сущностей
        /// </summary>
        [HttpGet]
        [DisableRequestSizeLimit]
        [ProducesResponseType<List<ApiServiceSummaryDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllApiServices()
        {
            try
            {
                var result = await _dynamicApiConfigurationService.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        /// <summary>
        /// Возвращает API-сервис по имени
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса</param>
        [HttpGet("{apiServiceName}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType<ApiServiceDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByNameApiService(
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")]
            string apiServiceName)
        {
            try
            {
                var result = await _dynamicApiConfigurationService.GetByIdAsync(apiServiceName);

                if (result == null)
                    return NotFound($"API-сервис {apiServiceName} не найден");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        /// <summary>
        /// Добавляет новый API-сервис
        /// </summary>
        /// <param name="apiServiceDto">Имя API-сервиса</param>
        [HttpPost]
        [DisableRequestSizeLimit]
        [ProducesResponseType<ApiServiceDto>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddApiService([FromBody] ApiServiceDto apiServiceDto)
        {
            try
            {
                var result = await _dynamicApiConfigurationService.CreateAsync(apiServiceDto);

                if (result == null)
                    return Conflict($"API-сервис {apiServiceDto.Name} уже существует");

                return CreatedAtAction(nameof(GetByNameApiService), new { apiServiceName = result.Name }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        /// <summary>
        /// Изменяет API-сервис
        /// </summary>
        /// <param name="apiServiceName">Имя изменяемого API-сервиса</param>
        /// <param name="apiServiceDto">Новые данные для API-сервиса</param>
        [HttpPut("{apiServiceName}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType<ApiServiceDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateApiService(
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")]
            [StringLength(200, ErrorMessage = "Имя не может быть больше 200 символов")]
            string apiServiceName, [FromBody] ApiServiceDto apiServiceDto)
        {
            try
            {
                var result = await _dynamicApiConfigurationService.UpdateAsync(apiServiceName, apiServiceDto);
                if (result == null)
                    return Conflict($"API-сервис с именем {apiServiceDto.Name} уже существует");

                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ConflictException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        /// <summary>
        /// Удаляет API-сервис
        /// </summary>
        /// <param name="apiServiceName">Имя изменяемого API-сервиса</param>
        [HttpDelete("{apiServiceName}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteApiService(
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")]
            string apiServiceName)
        {
            try
            {
                await _dynamicApiConfigurationService.DeleteAsync(apiServiceName);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        /// <summary>
        /// Меняет активность API-сервисов
        /// </summary>
        /// <param name="isActive">True если надо сделать активным API-сервис, false - неактивным</param>
        /// <param name="apiServiceName">Имя API-сервиса</param>
        [HttpPatch("{apiServiceName}/{isActive}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeActiveStatusApiService(bool isActive, string apiServiceName)
        {
            try
            {
                await _dynamicApiConfigurationService.ChangeActiveStatusAsync(apiServiceName, isActive);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }
    }
}

