﻿using ApiEasier.Bll.Dto;

namespace ApiEasier.Bll.Interfaces.ApiConfigure
{
    public interface IDynamicEntityConfigurationService
    {
        Task<bool> CreateAsync(string apiServiceName, ApiEntityDto entity);
        Task<bool> UpdateAsync(string apiServiceName,string entityName, ApiEntityDto entity);
        Task<bool> DeleteAsync(string apiServiceName, string id);
        public Task<bool> ChangeActiveStatusAsync(string apiServiceName, string entityName, bool status);
    }
}
