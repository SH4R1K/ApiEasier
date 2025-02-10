using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.Converter;
using ApiEasier.Dm.Models;

namespace ApiEasier.Bll.Converters
{
    public class ApiEntityToDtoSummaryConverter : IConverter<ApiEntity, ApiEntitySummaryDto>
    {
        public ApiEntitySummaryDto Convert(ApiEntity apiEntity) => new()
        {
            Name = apiEntity.Name,
            IsActive = apiEntity.IsActive,
            Structure = apiEntity.Structure,
        };
    }
}
