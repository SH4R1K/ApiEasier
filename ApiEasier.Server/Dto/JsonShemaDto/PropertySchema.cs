using System.Text.Json.Serialization;

namespace ApiEasier.Server.Dto.JsonShemaDto
{
    public class PropertySchema
    {
        [JsonPropertyName("type")]
        public object Type { get; set; } // Can be a string or an array of strings

        [JsonPropertyName("format")]
        public string Format { get; set; }

        [JsonPropertyName("maximum")]
        public int? Maximum { get; set; }

        [JsonPropertyName("minimum")]
        public int? Minimum { get; set; }

        [JsonPropertyName("oneOf")]
        public List<OneOfSchema> OneOf { get; set; }

        [JsonPropertyName("items")]
        public ReferenceOrPropertySchema Items { get; set; }

        [JsonPropertyName("$ref")]
        public string Ref { get; set; }

        [JsonPropertyName("enum")]
        public List<object> Enum { get; set; }

        [JsonPropertyName("x-enumNames")]
        public List<string> XEnumNames { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool? AdditionalProperties { get; set; }

        [JsonPropertyName("properties")]
        public Dictionary<string, PropertySchema> Properties { get; set; }
    }
}
