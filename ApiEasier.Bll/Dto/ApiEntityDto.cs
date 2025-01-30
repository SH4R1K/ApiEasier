namespace ApiEasier.Bll.Dto
{
    public class ApiEntityDto
    {
        // <summary>
        /// Имя сущности API.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Указывает, активна ли сущность API.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Структура сущности API.
        /// </summary>
        public object? Structure { get; set; } = null;

        /// <summary>
        /// Список конечных точек, связанных с сущностью API.
        /// </summary>
        public List<ApiActionDto> Actions { get; set; } = new List<ApiActionDto>();
    }
}
