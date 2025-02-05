using System.ComponentModel.DataAnnotations;

namespace ApiEasier.Server.Models
{
    /// <summary>
    /// Модель для представления действия API.
    /// </summary>
    public class ApiAction
    {
        /// <summary>
        /// Маршрут для действия API.
        /// </summary>
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")]
        public string Route { get; set; }

        /// <summary>
        /// Тип ответа, ожидаемого от действия API.
        /// </summary>
        public TypeResponse Type { get; set; }

        /// <summary>
        /// Указывает, активно ли действие API.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
