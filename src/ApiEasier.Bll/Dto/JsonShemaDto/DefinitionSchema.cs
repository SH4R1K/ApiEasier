using System.Text.Json.Serialization;

namespace ApiEasier.Bll.Dto.JsonShemaDto
{
    public class DefinitionSchema
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("additionalProperties")]
        public bool? AdditionalProperties { get; set; }

        [JsonPropertyName("properties")]
        public Dictionary<string, PropertySchema> Properties { get; set; }

        [JsonPropertyName("oneOf")]
        public List<OneOfSchema> OneOf { get; set; }
    }
}
