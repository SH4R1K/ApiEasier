using ApiEasier.Server.Models;

namespace ApiEasier.Server.Dto
{
    public class ApiServiceDto
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public List<ApiEntity> Entities { get; set; } = new List<ApiEntity>();
    }
}
