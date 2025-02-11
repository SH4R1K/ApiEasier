using ApiEasier.Server.Dto.JsonShemaDto;
using System.ComponentModel.DataAnnotations;

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
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")]
        public string? Name { get; set; }

        /// <summary>
        /// Указывает, активна ли сущность API.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Структура сущности API.
        /// </summary>
        public JsonSchema? Structure { get; set; } = null!;

        /// <summary>
        /// Список конечных точек, связанных с сущностью API.
        /// </summary>
        public List<ApiAction> Actions { get; set; } = new List<ApiAction>();
    }
}
