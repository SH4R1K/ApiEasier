using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiEasier.Api.Converters
{
    public class JsonStringEnumCamelCaseConverter<T> : JsonConverter<T> where T : Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var enumString = reader.GetString();
            return (T)Enum.Parse(typeof(T), enumString, true);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var enumString = value.ToString();
            var camelCaseString = char.ToLower(enumString[0]) + enumString[1..];
            writer.WriteStringValue(camelCaseString);
        }
    }
}
