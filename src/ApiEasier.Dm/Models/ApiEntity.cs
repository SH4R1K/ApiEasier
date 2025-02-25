using ApiEasier.Dm.Models.JsonShema;
using System.Text.Json.Serialization;

namespace ApiEasier.Dm.Models
{
    /// <summary>
    /// Модель для представления сущности API.
    /// </summary>
    public class ApiEntity
    {
        /// <summary>
        /// Имя сущности API.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Указывает, активна ли сущность API.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Структура сущности API.
        /// </summary>
        public JsonSchema? Structure { get; set; } = null;

        /// <summary>
        /// Список конечных точек, связанных с сущностью API.
        /// </summary>
        public List<ApiEndpoint> Endpoints { get; set; } = new List<ApiEndpoint>();
    }
}
