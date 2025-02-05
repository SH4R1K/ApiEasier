using ApiEasier.Bll.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEasier.Bll.Interfaces.ApiConfigure
{
    public interface IDynamicEndpointConfigurationService
    {
        Task<bool> CreateAsync(string apiServiceName, string apiEntityName, ApiEndpointDto apiEndpointDto);
        Task<bool> UpdateAsync(string apiServiceName, string apiEntityName, ApiEndpointDto apiEndpointDto);
        Task<bool> DeleteAsync(string apiServiceName, string apiEntityName, string id);
        public Task<bool> ChangeActiveStatusAsync(string apiServiceName, string entityName, string endpointName, bool status);
    }
}
