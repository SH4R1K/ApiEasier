using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.Converter;
using ApiEasier.Dm.Models;
using System.Text;

namespace ApiEasier.Bll.Converters
{
    public class DtoToApiServiceConverter : IConverter<ApiServiceDto, ApiService>
    {
        private readonly IConverter<ApiEntityDto, ApiEntity> _dtoToApiEntityConverter;

        public DtoToApiServiceConverter(IConverter<ApiEntityDto, ApiEntity> dtoToApiEntityConverter)
        {
            _dtoToApiEntityConverter = dtoToApiEntityConverter;
        }

        public ApiService Convert(ApiServiceDto apiServiceDto) => new()
        {
            Name = apiServiceDto.Name,
            IsActive = apiServiceDto.IsActive,
            Description = apiServiceDto.Description,
            Entities = apiServiceDto.Entities.Select(_dtoToApiEntityConverter.Convert).ToList(),
        };
    }
}
