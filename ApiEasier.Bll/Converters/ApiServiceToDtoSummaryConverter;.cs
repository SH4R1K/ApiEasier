using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.Converter;
using ApiEasier.Dm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEasier.Bll.Converters
{
    public class ApiServiceToDtoSummaryConverter_ : IConverter<ApiService, ApiServiceSummaryDto>
    {
        public ApiServiceSummaryDto Convert(ApiService apiService) => new()
        {
            Name = apiService.Name,
            Description = apiService.Description,
            IsActive = apiService.IsActive,
        };
    }
}
