namespace ApiEasier.Dm.Models
{
    /// <summary>
    /// Модель для представления действия API.
    /// </summary>
    public class ApiEndpoint
    {
        /// <summary>
        /// Маршрут для действия API.
        /// </summary>
        public required string Route { get; set; }

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
