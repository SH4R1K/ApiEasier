using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEasier.Dal.Interfaces
{
    /// <summary>
    /// Позволяет работать с источниками данных для эмуляции
    /// </summary>
    public interface IResourceRepository
    {
        /// <summary>
        /// Удаляет источники данных по имени API-сервиса
        /// </summary>
        /// <param name="id">Идентиффикатор API-сервис</param>
        /// <returns>Прошло ли удаление</returns>
        public Task<bool> DeleteByApiNameAsync(string id);
        public Task<bool> DeleteByApiEntityNameAsync(string id);

        /// <summary>
        /// Обновляет источники данных по имени API-сервиса
        /// </summary>
        /// <param name="id">Текущий идентификатор API-сервиса</param>
        /// <param name="newId">Новый идентификатор API-сервиса</param>
        /// <returns>Прошло ли обновление</returns>
        public Task<bool> UpdateByApiNameAsync(string id, string newId);
        public Task<bool> UpdateByApiEntityNameAsync(string apiServiceNmae, string id, string newId);
        public Task DeleteUnusedResources(List<string> apiServiceNames);
    }
}
