using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.Converter;
using ApiEasier.Dm.Models;

namespace ApiEasier.Bll.Converters
{
    public class ApiEndpointToDtoConverter : IConverter<ApiEndpoint, ApiEndpointDto>
    {
        public ApiEndpointDto Convert(ApiEndpoint apiEndpoint) => new()
        {
            Route = apiEndpoint.Route,
            IsActive = apiEndpoint.IsActive,
            Type = apiEndpoint.Type,
        };
    }
}
