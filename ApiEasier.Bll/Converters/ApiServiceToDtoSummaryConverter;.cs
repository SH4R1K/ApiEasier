using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.Converter;
using ApiEasier.Dm.Models;

namespace ApiEasier.Bll.Converters
{
    public class ApiServiceToDtoSummaryConverter : IConverter<ApiService, ApiServiceSummaryDto>
    {
        public ApiServiceSummaryDto Convert(ApiService apiService) => new()
        {
            Name = apiService.Name,
            IsActive = apiService.IsActive,
            Description = apiService.Description,
        };
    }
}
