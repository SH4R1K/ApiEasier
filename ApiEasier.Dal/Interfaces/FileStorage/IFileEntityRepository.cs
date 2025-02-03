using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Interfaces.FileStorage
{
    public interface IFileEntityRepository
    {
        Task<bool> CreateAsync(string apiServiceName, ApiEntity apiEntity);
        Task<bool> UpdateAsync(string apiServiceName, string id, ApiEntity apiEntity);
        bool Delete(string apiServiceName, string id);
    }
}
