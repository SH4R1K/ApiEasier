using System.ComponentModel.DataAnnotations;

namespace ApiEasier.Bll.Dto
{
    /// <summary>
    /// DTO для представления API-сервиса с именем.
    /// </summary>
    public class ApiServiceDto
    {
        /// <summary>
        /// Имя API-сервиса.
        /// </summary>
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")]
        public required string Name { get; set; }

        /// <summary>
        /// Указывает, активен ли API-сервис.
        /// </summary>
        [Required]
        public bool IsActive { get; set; }


        private string? _description;
        /// <summary>
        /// Описание API-сервиса.
        /// </summary>
        [StringLength(255)]
        public string? Description
        {
            get => _description;
            set => _description = value?.Trim();
        }

        /// <summary>
        /// Список сущностей, связанных с API-сервисом.
        /// </summary>
        public List<ApiEntityDto> Entities { get; set; } = new List<ApiEntityDto>();
    }
}