namespace ApiEasier.Server.Models
{
    public class ApiService
    {

        public bool IsActive { get; set; }

        public List<ApiEntity> Entities { get; set; } = new List<ApiEntity>();
    }
}
