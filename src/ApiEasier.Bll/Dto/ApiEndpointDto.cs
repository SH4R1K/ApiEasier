using ApiEasier.Dm.Models;

namespace ApiEasier.Bll.Dto
{
    public class ApiEndpointDto
    {
        /// <summary>
        /// Маршрут для действия API.
        /// </summary>
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
