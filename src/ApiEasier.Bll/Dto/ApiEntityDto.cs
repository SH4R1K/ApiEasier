using ApiEasier.Dm.Models.JsonShema;
using System.ComponentModel.DataAnnotations;

namespace ApiEasier.Bll.Dto
{
    /// <summary>
    /// DTO для представления сущности
    /// </summary>
    public class ApiEntityDto
    {
        /// <summary>
        /// Имя сущности API.
        /// </summary>
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")]
        public required string Name { get; set; }

        /// <summary>
        /// Указывает, активна ли сущность API.
        /// </summary>
        [Required]
        public bool IsActive { get; set; }

        /// <summary>
        /// Структура сущности API.
        /// </summary>
        public JsonSchema? Structure { get; set; } = null;

        /// <summary>
        /// Список конечных точек, связанных с сущностью API.
        /// </summary>
        public List<ApiEndpointDto> Endpoints { get; set; } = new List<ApiEndpointDto>();
    }
}
