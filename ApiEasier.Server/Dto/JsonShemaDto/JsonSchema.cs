using System.Text.Json.Serialization;

namespace ApiEasier.Server.Dto.JsonShemaDto
{
    public class JsonSchema
    {
        [JsonPropertyName("$schema")]
        public string Schema { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool? AdditionalProperties { get; set; }

        [JsonPropertyName("required")]
        public List<string> Required { get; set; }

        [JsonPropertyName("properties")]
        public Dictionary<string, PropertySchema> Properties { get; set; }

        [JsonPropertyName("definitions")]
        public Dictionary<string, DefinitionSchema> Definitions { get; set; }
    }
}
