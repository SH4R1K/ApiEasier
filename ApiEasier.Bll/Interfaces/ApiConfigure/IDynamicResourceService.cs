using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEasier.Bll.Interfaces.ApiConfigure
{
    public interface IDynamicResourceService
    {
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateNameAsync(string resourceName, string newResourceName);
    }
}
