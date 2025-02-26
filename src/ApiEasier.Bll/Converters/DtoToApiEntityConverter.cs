using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.Converter;
using ApiEasier.Dm.Models;
using System.Text;

namespace ApiEasier.Bll.Converters
{
    public class DtoToApiEntityConverter : IConverter<ApiEntityDto, ApiEntity>
    {
        private readonly IConverter<ApiEndpointDto, ApiEndpoint> _dtoToApiEndpointConverter;

        public DtoToApiEntityConverter(IConverter<ApiEndpointDto, ApiEndpoint> dtoToApiEndpointConverter)
        {
            _dtoToApiEndpointConverter = dtoToApiEndpointConverter;
        }

        public ApiEntity Convert(ApiEntityDto apiEntityDto) => new()
        {
            Name = apiEntityDto.Name,
            Structure = apiEntityDto.Structure,
            IsActive = apiEntityDto.IsActive,
            Endpoints = apiEntityDto.Endpoints.Select(_dtoToApiEndpointConverter.Convert).ToList(),
        };
    }
}
