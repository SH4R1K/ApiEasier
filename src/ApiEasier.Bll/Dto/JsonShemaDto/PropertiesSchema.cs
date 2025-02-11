using System.Text.Json.Serialization;

namespace ApiEasier.Bll.Dto.JsonShemaDto
{
    public class PropertiesSchema
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("required")]
        public List<string>? Required { get; set; }

        [JsonPropertyName("properties")]
        public Dictionary<string, PropertySchema> Properties { get; set; }
    }
}
