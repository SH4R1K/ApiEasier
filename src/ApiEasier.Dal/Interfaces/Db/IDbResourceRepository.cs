using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEasier.Dal.Interfaces.Db
{
    public interface IDbResourceRepository
    {
        public Task<bool> DeleteAsync(string id);
    }
}
