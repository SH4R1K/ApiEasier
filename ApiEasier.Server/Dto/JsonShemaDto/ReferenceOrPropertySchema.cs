using System.Text.Json.Serialization;

namespace ApiEasier.Server.Dto.JsonShemaDto
{
    public class ReferenceOrPropertySchema
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
