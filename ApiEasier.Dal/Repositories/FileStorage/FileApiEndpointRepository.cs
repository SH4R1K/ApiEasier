using ApiEasier.Dal.Interfaces.FileStorage;
using ApiEasier.Dal.Interfaces.Helpers;
using ApiEasier.Dm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEasier.Dal.Repositories.FileStorage
{
    public class FileApiEndpointRepository : IFileApiEndpointRepository
    {
        private readonly IFileHelper _jsonFileHelper;

        public FileApiEndpointRepository(IFileHelper jsonFileHelper)
        {
            _jsonFileHelper = jsonFileHelper;
        }

        public Task<bool> ChangeActiveStatusAsync(string apiServiceName, string apiEntityName, string id, bool status)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateAsync(string apiServiceName, string apiEntityName, ApiEndpoint apiEndpoint)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string apiServiceName, string apiEntityName, string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(string apiServiceName, string apiEntityName, string id, ApiEndpoint apiEndpoint)
        {
            throw new NotImplementedException();
        }
    }
}
