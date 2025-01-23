using ApiEasier.Server.Converters;
using System.Text.Json.Serialization;

namespace ApiEasier.Server.Models
{
    [JsonConverter(typeof(JsonStringEnumCamelCaseConverter<TypeResponse>))]
    /// <summary>
    /// Перечисление, представляющее типы ответов API.
    /// </summary>
    public enum TypeResponse
    {
        /// <summary>
        /// Тип ответа для операции получения данных.
        /// </summary>
        Get,

        /// <summary>
        /// Тип ответа для операции создания данных.
        /// </summary>
        Post,

        /// <summary>
        /// Тип ответа для операции обновления данных.
        /// </summary>
        Put,

        /// <summary>
        /// Тип ответа для операции удаления данных.
        /// </summary>
        Delete,

        /// <summary>
        /// Тип ответа для операции получения данных по индексу.
        /// </summary>
        GetByIndex
    }
}
