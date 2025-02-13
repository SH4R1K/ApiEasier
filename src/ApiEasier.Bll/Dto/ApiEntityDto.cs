using ApiEasier.Dm.Models.JsonShema;

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
        public JsonSchema? Structure { get; set; } = null;

        /// <summary>
        /// Список конечных точек, связанных с сущностью API.
        /// </summary>
        public List<ApiEndpointDto> Endpoints { get; set; } = new List<ApiEndpointDto>();
    }
}
