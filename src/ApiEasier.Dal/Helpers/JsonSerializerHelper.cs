using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiEasier.Dal.Helpers
{
    /// <summary>
    /// Позволяет десерилизовывать и сериализовывать JSON, обертка над стандартным JsonSerializer для указания своих опций сериализации
    /// </summary>
    public class JsonSerializerHelper
    {
        /// <summary>
        /// Опции сериализации (сейчас переводит название свойств в camelCase, делает его более читаемым и игнорирует 
        /// свойство c null значениями)
        /// </summary>
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

        /// <inheritdoc cref="JsonSerializer.Serialize"/>
        public static string Serialize<T>(T? obj)
        {
            return JsonSerializer.Serialize(obj, Options);
        }
    }
}
