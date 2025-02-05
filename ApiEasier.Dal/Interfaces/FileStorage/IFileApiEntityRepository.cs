using ApiEasier.Dm.Models;

namespace ApiEasier.Dal.Interfaces.FileStorage
{
    public interface IFileApiEntityRepository
    {
        Task<bool> CreateAsync(string apiServiceName, ApiEntity apiEntity);
        Task<bool> UpdateAsync(string apiServiceName, string id, ApiEntity apiEntity);
        Task<bool> DeleteAsync(string apiServiceName, string id);
        Task<bool> ChangeActiveStatusAsync(string apiServiceName, string id, bool status);
    }
}
