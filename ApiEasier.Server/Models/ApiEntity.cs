namespace ApiEasier.Server.Models
{
    public class ApiEntity
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public object? Structure { get; set; }

        public List<ApiAction> Actions { get; set; } = new List<ApiAction>();
    }
}
