namespace ApiEasier.Server.Models
{
    public class ApiAction
    {
        public string Route { get; set; }

        public TypeResponse Type { get; set; }

        public bool IsActive { get; set; }
    }
}
