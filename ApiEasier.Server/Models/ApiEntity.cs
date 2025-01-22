using NJsonSchema;
using System.Collections.Generic;

namespace ApiEasier.Server.Models
{
    /// <summary>
    /// Модель для представления сущности API.
    /// </summary>
    public class ApiEntity
    {
        /// <summary>
        /// Имя сущности API.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Указывает, активна ли сущность API.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Структура сущности API.
        /// </summary>
        public object? Structure { get; set; }

        /// <summary>
        /// Список конечных точек, связанных с сущностью API.
        /// </summary>
        public List<ApiAction> Actions { get; set; } = new List<ApiAction>();
    }
}
