using System.Text.Json.Serialization;

namespace ApiEasier.Dm.Models
{
    /// <summary>
    /// Модель для представления API-сервиса.
    /// </summary>
    public class ApiService
    {   
        /// <summary>
        /// Имя API-сервиса.
        /// </summary>
        [JsonIgnore]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Указывает, активен ли API-сервис.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Описание API-сервиса.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Список сущностей, связанных с API-сервисом.
        /// </summary>
        public List<ApiEntity> Entities { get; set; } = new List<ApiEntity>();
    }
}
