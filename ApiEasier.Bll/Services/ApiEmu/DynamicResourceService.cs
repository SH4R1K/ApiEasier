using ApiEasier.Bll.Interfaces.ApiEmu;
using ApiEasier.Bll.Validators;
using ApiEasier.Dal.Interfaces.Db;
using ApiEasier.Dm.Models;

namespace ApiEasier.Bll.Services.ApiEmu
{
    public class DynamicResourceService : IDynamicResourceService
    {
        private readonly DynamicResourceValidator _validator;
        private readonly IDbResourceDataRepository _dbResourceDataRepository;

        public DynamicResourceService(DynamicResourceValidator validator, IDbResourceDataRepository dbResourceDataRepository)
        {
            _validator = validator;
            _dbResourceDataRepository = dbResourceDataRepository;
        }

        public async Task<List<DynamicResource>?> GetAsync(string apiName, string apiEntityName, string endpoint, string? filters)
        {
            var (isValid, _, _) = await _validator.ValidateApiAsync(apiName, apiEntityName, endpoint, "get");
            if (!isValid)
                return null;

            string resourceName = apiName.Trim().Replace(" ", "") + "_" + apiEntityName.Trim().Replace(" ", "");
            var result = await _dbResourceDataRepository.GetAllDataAsync(resourceName);

            return result;
        }

        public async Task<DynamicResource> GetByIdAsync(string apiName, string apiEntityName, string endpoint, string id, string? filters)
        {

            var (isValid, _, _) = await _validator.ValidateApiAsync(apiName, apiEntityName, endpoint, "get");
            if (!isValid)
                return null;

            string resourceName = apiName.Trim().Replace(" ", "") + "_" + apiEntityName.Trim().Replace(" ", "");
            var result = await _dbResourceDataRepository.GetDataByIdAsync(resourceName, id);

            return result;
        }

        public async Task<DynamicResource> AddAsync(string apiName, string apiEntityName, string endpoint, object json)
        {
            string resourceName = apiName.Trim().Replace(" ", "") + "_" + apiEntityName.Trim().Replace(" ", "");

            return await _dbResourceDataRepository.CreateDataAsync(resourceName, json);
        }

        public async Task<bool> Delete(string apiName, string apiEntityName, string endpoint, string id)
        {

            var (isValid, _, entity) = await _validator.ValidateApiAsync(apiName, apiEntityName, endpoint, "get");
            if (!isValid)
                return false;

            string resourceName = apiName.Trim().Replace(" ", "") + "_" + apiEntityName.Trim().Replace(" ", "");
            var result = await _dbResourceDataRepository.DeleteDataAsync(resourceName, id);

            return result;
        }

        public async Task<DynamicResource> UpdateAsync(string apiName, string apiEntityName, string endpoint, string id, object json)
        {
            //Валидация API, сущности и пути
            var (isValid, _, entity) = await _validator.ValidateApiAsync(apiName, apiEntityName, endpoint, "put");
            if (!isValid)
                return null;

            // Валидация структуры для сущности
            isValid = await _validator.ValidateEntityStructureAsync(entity!, json);
            if (!isValid)
                return null;

            string resourceName = apiName.Trim().Replace(" ", "") + "_" + apiEntityName.Trim().Replace(" ", "");
            var result = await _dbResourceDataRepository.UpdateDataAsync(resourceName, id, json);

            return result;
        }
    }
}
