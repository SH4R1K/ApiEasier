namespace ApiEasier.Bll.Dto
{
    public class ApiServiceSummaryDto
    {
        /// <summary>
        /// Имя API-сервиса.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Указывает, активен ли API-сервис.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Описание API-сервиса.
        /// </summary>
        public string? Description { get; set; }
    }
}
