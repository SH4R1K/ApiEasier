using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiEasier.Dal.Helpers
{
    /// <summary>
    /// Позволяет десерилизовывать и сериализовывать JSON
    /// </summary>
    public class JsonSerializerHelper
    {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        /// <inheritdoc cref="JsonSerializer.Deserialize"/>
        public static T? Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, Options);
        }

        public static string Serialize<T>(T? obj)
        {
            return JsonSerializer.Serialize(obj, Options);
        }
    }
}
