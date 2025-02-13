using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEasier.Dal.Interfaces.Db
{
    public interface IDbResourceRepository
    {
        public Task<bool> DeleteByApiNameAsync(string id);
        public Task<bool> DeleteByApiEntityNameAsync(string id);
        public Task<bool> UpdateByApiNameAsync(string id, string newId);
        public Task<bool> UpdateByApiEntityNameAsync(string apiServiceNmae, string id, string newId);
        public Task DeleteUnusedResources();
    }
}
