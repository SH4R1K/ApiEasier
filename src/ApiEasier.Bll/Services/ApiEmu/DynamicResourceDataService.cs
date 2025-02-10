using ApiEasier.Bll.Interfaces.ApiEmu;
using ApiEasier.Bll.Interfaces.Validators;
using ApiEasier.Bll.Validators;
using ApiEasier.Dal.Interfaces.Db;
using ApiEasier.Dm.Models;

namespace ApiEasier.Bll.Services.ApiEmu
{
    public class DynamicResourceDataService : IDynamicResourceDataService
    {
        private readonly IDynamicResourceValidator _validator;
        private readonly IDbResourceDataRepository _dbResourceDataRepository;

        public DynamicResourceDataService(IDynamicResourceValidator validator, IDbResourceDataRepository dbResourceDataRepository)
        {
            _validator = validator;
            _dbResourceDataRepository = dbResourceDataRepository;
        }

        private string GetResourceName(string apiName, string apiEntityName) 
            => apiName.Trim().Replace(" ", "") + "_" + apiEntityName.Trim().Replace(" ", "");

        public async Task<List<DynamicResourceData>?> GetAsync(string apiName, string apiEntityName, string endpoint, string? filters)
        {
            var (isValid, _, _) = await _validator.ValidateApiAsync(apiName, apiEntityName, endpoint, "get");
            if (!isValid)
                return null;

            var result = await _dbResourceDataRepository.GetAllDataAsync(GetResourceName(apiName, apiEntityName));

            return result;
        }

        public async Task<DynamicResourceData> GetByIdAsync(string apiName, string apiEntityName, string endpoint, string id, string? filters)
        {

            var (isValid, _, _) = await _validator.ValidateApiAsync(apiName, apiEntityName, endpoint, "getByIndex");
            if (!isValid)
                return null;

            var result = await _dbResourceDataRepository.GetDataByIdAsync(GetResourceName(apiName, apiEntityName), id);

            return result;
        }

        public async Task<DynamicResourceData?> AddAsync(string apiName, string apiEntityName, string endpoint, object json)
        {
            var (isValid, _, entity) = await _validator.ValidateApiAsync(apiName, apiEntityName, endpoint, "post");
            if (!isValid)
                return null;

            // Валидация структуры для сущности
            isValid = await _validator.ValidateEntityStructureAsync(entity!, json);
            if (!isValid)
                return null;

            return await _dbResourceDataRepository.CreateDataAsync(GetResourceName(apiName, apiEntityName), json);
        }

        public async Task<bool> Delete(string apiName, string apiEntityName, string endpoint, string id)
        {

            var (isValid, _, entity) = await _validator.ValidateApiAsync(apiName, apiEntityName, endpoint, "delete");
            if (!isValid)
                return false;

            var result = await _dbResourceDataRepository.DeleteDataAsync(GetResourceName(apiName, apiEntityName), id);

            return result;
        }

        public async Task<DynamicResourceData?> UpdateAsync(string apiName, string apiEntityName, string endpoint, string id, object json)
        {
            var (isValid, _, entity) = await _validator.ValidateApiAsync(apiName, apiEntityName, endpoint, "put");
            if (!isValid)
                return null;

            // Валидация структуры для сущности
            isValid = await _validator.ValidateEntityStructureAsync(entity!, json);
            if (!isValid)
                return null;

            var result = await _dbResourceDataRepository.UpdateDataAsync(GetResourceName(apiName, apiEntityName), id, json);

            return result;
        }
    }
}
