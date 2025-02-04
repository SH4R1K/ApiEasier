using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces;
using ApiEasier.Dm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
