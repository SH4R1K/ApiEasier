using ApiEasier.Bll.Interfaces.ApiEmu;
using ApiEasier.Bll.Interfaces.Validators;
using ApiEasier.Dal.Interfaces;
using ApiEasier.Dm.Models;

namespace ApiEasier.Bll.Services.ApiEmu
{
    /// <inheritdoc cref="IDynamicResourceDataService"/>
    public class DynamicResourceDataService : IDynamicResourceDataService
    {
        private readonly IDynamicResourceValidator _validator;
        private readonly IResourceDataRepository _resourceDataRepository;

        public DynamicResourceDataService(IDynamicResourceValidator validator, IResourceDataRepository resourceDataRepository)
        {
            _validator = validator;
            _resourceDataRepository = resourceDataRepository;
        }

        /// <summary>
        /// Возвращает имя ресурса, где будут хранится данные сущности, в виде имяApiСервиса_имяСущности
        /// </summary>
        /// <param name="apiName">Имя API-сервиса</param>
        /// <param name="apiEntityName">Имя сущности</param>
        /// <returns>Имя ресурса хранения данных сущности</returns>
        private string GetResourceName(string apiName, string apiEntityName) 
            => apiName.Trim().Replace(" ", "") + "_" + apiEntityName.Trim().Replace(" ", "");

        public async Task<List<DynamicResourceData>?> GetAsync(string apiName, string apiEntityName, string endpoint, string? filters)
        {
            var (isValid, _, _) = await _validator.ValidateApiAsync(apiName, apiEntityName, endpoint, "get");
            if (!isValid)
                return null;

            var result = await _resourceDataRepository.GetAllDataAsync(GetResourceName(apiName, apiEntityName), filters);

            return result;
        }

        public async Task<DynamicResourceData?> GetByIdAsync(string apiName, string apiEntityName, string endpoint, string id)
        {

            var (isValid, _, _) = await _validator.ValidateApiAsync(apiName, apiEntityName, endpoint, "getByIndex");
            if (!isValid)
                return null;

            var result = await _resourceDataRepository.GetDataByIdAsync(GetResourceName(apiName, apiEntityName), id);

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

            return await _resourceDataRepository.CreateDataAsync(GetResourceName(apiName, apiEntityName), json);
        }

        public async Task<bool> Delete(string apiName, string apiEntityName, string endpoint, string id)
        {

            var (isValid, _, entity) = await _validator.ValidateApiAsync(apiName, apiEntityName, endpoint, "delete");
            if (!isValid)
                return false;

            var result = await _resourceDataRepository.DeleteDataAsync(GetResourceName(apiName, apiEntityName), id);

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

            var result = await _resourceDataRepository.UpdateDataAsync(GetResourceName(apiName, apiEntityName), id, json);

            return result;
        }
    }
}
