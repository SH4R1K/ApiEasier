using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEasier.Dal.Interfaces.Db
{
    public interface IDbResourceRepository
    {
        Task<string> GetAsync(string resourceName);
        Task<bool> DeleteAsync(string resourceName);
    }
}
