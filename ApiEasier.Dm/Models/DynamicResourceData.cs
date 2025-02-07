using System.Text.Json.Nodes;

namespace ApiEasier.Dm.Models
{
    /// <summary>
    /// Модель данных, содеражащихся в динамическом ресурсе (в mongoDb - в коллекции)
    /// </summary>
    public class DynamicResourceData
    {
        public JsonNode Data { get; set; }
    }
}
