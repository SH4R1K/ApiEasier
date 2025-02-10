using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.Converter;
using ApiEasier.Dm.Models;

namespace ApiEasier.Bll.Converters
{
    public class ApiServiceToDtoConverter(
        IConverter<ApiEntity, ApiEntityDto> apiEntityToDtoConverter) : IConverter<ApiService, ApiServiceDto>
    {
        private readonly IConverter<ApiEntity, ApiEntityDto> _apiEntityToDtoConverter = apiEntityToDtoConverter;

        public ApiServiceDto Convert(ApiService apiService) => new()
        {
            Name = apiService.Name,
            Description = apiService.Description,
            IsActive = apiService.IsActive,
            Entities = apiService.Entities.Select(_apiEntityToDtoConverter.Convert).ToList(),
        };
    }
}
