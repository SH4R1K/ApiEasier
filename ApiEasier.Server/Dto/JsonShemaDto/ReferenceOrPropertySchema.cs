using System.Text.Json.Serialization;

namespace ApiEasier.Server.Dto.JsonShemaDto
{
    public class ReferenceOrPropertySchema
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("$ref")]
        public string? Ref { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
