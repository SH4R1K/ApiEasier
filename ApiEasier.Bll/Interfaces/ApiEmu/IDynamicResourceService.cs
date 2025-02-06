using ApiEasier.Dm.Models.Dynamic;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEasier.Bll.Interfaces.ApiEmu
{
    public interface IDynamicResourceService
    {
        public Task<List<DynamicResource>> GetAsync(string apiName, string apiEntityName, string endpoint, string? filters);
        public Task<DynamicResource> GetByIdAsync(string apiName, string apiEntityName, string endpoint, string id, string? filters);
        public Task<DynamicResource> AddAsync(string apiName, string apiEntityName, string endpoint, object json);
        public Task<DynamicResource> UpdateAsync(string apiName, string apiEntityName, string endpoint, string id, object json);
        public Task<bool> Delete(string apiName, string apiEntityName, string endpoint, string id);
    }
}
