using ApiEasier.Dm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEasier.Dal.Interfaces.Db
{
    public interface IDbResourceRepository
    {
        Task<List<string>> GetNamesAsync();
        Task<bool> DeleteAsync(string resourceName);
        Task<bool> UpdateNameAsync(string oldName, string name);
    }
}
