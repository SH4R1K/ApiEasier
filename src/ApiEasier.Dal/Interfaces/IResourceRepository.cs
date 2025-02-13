using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEasier.Dal.Interfaces
{
    public interface IResourceRepository
    {
        public Task<bool> DeleteByApiNameAsync(string id);
        public Task<bool> DeleteByApiEntityNameAsync(string id);
        public Task<bool> UpdateByApiNameAsync(string id, string newId);
        public Task<bool> UpdateByApiEntityNameAsync(string apiServiceNmae, string id, string newId);
        public Task DeleteUnusedResources(List<string> apiServiceNames);
    }
}
