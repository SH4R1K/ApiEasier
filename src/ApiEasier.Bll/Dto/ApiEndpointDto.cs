using ApiEasier.Dm.Models;
using System.ComponentModel.DataAnnotations;

namespace ApiEasier.Bll.Dto
{
    public class ApiEndpointDto
    {
        /// <summary>
        /// Маршрут для действия API.
        /// </summary>
        [Required]
        public required string Route { get; set; }

        /// <summary>
        /// Тип ответа, ожидаемого от действия API.
        /// </summary>
        [Required]
        public TypeResponse Type { get; set; }

        /// <summary>
        /// Указывает, активно ли действие API.
        /// </summary>
        [Required]
        public bool IsActive { get; set; }
    }
}
