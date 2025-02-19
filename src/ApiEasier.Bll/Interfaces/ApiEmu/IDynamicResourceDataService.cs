using ApiEasier.Dm.Models;

namespace ApiEasier.Bll.Interfaces.ApiEmu
{
    public interface IDynamicResourceDataService
    {
        public Task<List<DynamicResourceData>?> GetAsync(string apiName, string apiEntityName, string endpoint, string? filters);
        public Task<DynamicResourceData?> GetByIdAsync(string apiName, string apiEntityName, string endpoint, string id);
        public Task<DynamicResourceData?> AddAsync(string apiName, string apiEntityName, string endpoint, object json);
        public Task<DynamicResourceData?> UpdateAsync(string apiName, string apiEntityName, string endpoint, string id, object json);
        public Task<bool> Delete(string apiName, string apiEntityName, string endpoint, string id);
    }
}
