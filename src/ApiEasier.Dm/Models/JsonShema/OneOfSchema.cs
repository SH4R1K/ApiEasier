using System.Text.Json.Serialization;

namespace ApiEasier.Dm.Models.JsonShema
{
    public class OneOfSchema
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
