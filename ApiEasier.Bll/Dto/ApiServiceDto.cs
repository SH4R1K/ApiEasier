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