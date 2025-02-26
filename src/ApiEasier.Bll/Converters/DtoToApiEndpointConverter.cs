using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.Converter;
using ApiEasier.Dm.Models;

namespace ApiEasier.Bll.Converters
{
    public class DtoToApiEndpointConverter : IConverter<ApiEndpointDto, ApiEndpoint>
    {
        public ApiEndpoint Convert(ApiEndpointDto apiEndpointDto) => new()
        {
            Route = apiEndpointDto.Route,
            Type = apiEndpointDto.Type,
            IsActive = apiEndpointDto.IsActive,
        };
    }
}
