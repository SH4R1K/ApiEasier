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
    public class ApiActionToDtoConverter : IConverter<ApiAction, ApiActionDto>
    {
        public ApiActionDto Convert(ApiAction apiAction) => new()
        {
            Route = apiAction.Route,
            IsActive = apiAction.IsActive,
            Type = apiAction.Type,
        };
    }
}
