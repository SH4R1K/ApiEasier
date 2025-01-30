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
    public class ApiEntityToDtoConverter(
        IConverter<ApiAction, ApiActionDto> apiActionToDtoConverter) : IConverter<ApiEntity, ApiEntityDto>
    {
        private readonly IConverter<ApiAction, ApiActionDto> _apiActionToDtoConverter = apiActionToDtoConverter;

        public ApiEntityDto Convert(ApiEntity apiEntity) => new()
        {
            Name = apiEntity.Name,
            Structure = apiEntity.Structure,
            IsActive = apiEntity.IsActive,
            Actions = apiEntity.Actions.Select(_apiActionToDtoConverter.Convert).ToList(),
        };
    }
}
