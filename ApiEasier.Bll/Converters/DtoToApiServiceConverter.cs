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
    public class DtoToApiServiceConverter : IConverter<ApiServiceDto, ApiService>
    {
        public ApiService Convert(ApiServiceDto apiServiceDto) => new()
        {
            Name = apiServiceDto.Name,
            IsActive = apiServiceDto.IsActive,
            Description = apiServiceDto.Description,
            Entities = apiServiceDto.Entities,
        };
    }
}
