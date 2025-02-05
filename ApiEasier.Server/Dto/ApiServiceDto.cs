using ApiEasier.Server.Models;
using System.ComponentModel.DataAnnotations;

namespace ApiEasier.Server.Dto
{
    /// <summary>
    /// DTO для представления API-сервиса с именем.
    /// </summary>
    public class ApiServiceDto
    {

        /// <summary>
        /// Имя API-сервиса.
        /// </summary>
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")]
        public string Name { get; set; }

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