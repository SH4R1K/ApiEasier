using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.Converter;
using ApiEasier.Dm.Models;
using System.Text;

namespace ApiEasier.Bll.Converters
{
    public class ApiEntityToDtoConverter(
        IConverter<ApiEndpoint, ApiEndpointDto> apiEndpointToDtoConverter) : IConverter<ApiEntity, ApiEntityDto>
    {
        private readonly IConverter<ApiEndpoint, ApiEndpointDto> _apiEndpointToDtoConverter = apiEndpointToDtoConverter;

        public ApiEntityDto Convert(ApiEntity apiEntity) => new()
        {
            Name = apiEntity.Name,
            Structure = apiEntity.Structure,
            IsActive = apiEntity.IsActive,
            Endpoints = apiEntity.Endpoints.Select(_apiEndpointToDtoConverter.Convert).ToList(),
        };
    }
}
