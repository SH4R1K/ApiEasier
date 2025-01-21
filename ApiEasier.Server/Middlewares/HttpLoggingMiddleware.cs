using System.Text;

namespace ApiEasier.Server.Middlewares
{
    public class HttpLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public HttpLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<HttpLoggingMiddleware>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Логируем запрос
            await LogRequestAsync(context);

            // Логируем ответ
            await LogResponseAsync(context);
        }

        private async Task LogRequestAsync(HttpContext context)
        {
            context.Request.EnableBuffering(); // Позволяет повторно читать тело запроса
            var body = await ReadStreamAsync(context.Request.Body);

            _logger.LogInformation("HTTP Request:");
            _logger.LogInformation($"Method: {context.Request.Method}");
            _logger.LogInformation($"Path: {context.Request.Path}");
            _logger.LogInformation($"Query: {context.Request.QueryString}");
            _logger.LogInformation($"Headers: {FormatHeaders(context.Request.Headers)}");
            _logger.LogInformation($"Body: {body}");

            context.Request.Body.Position = 0; // Сбрасываем позицию потока
        }

        private async Task LogResponseAsync(HttpContext context)
        {
            // Сохраняем оригинальный поток ответа
            var originalBodyStream = context.Response.Body;

            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context); // Выполняем следующий middleware

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation("HTTP Response:");
            _logger.LogInformation($"Status Code: {context.Response.StatusCode}");
            _logger.LogInformation($"Headers: {FormatHeaders(context.Response.Headers)}");
            _logger.LogInformation($"Body: {responseBody}");

            // Возвращаем поток ответа
            await responseBodyStream.CopyToAsync(originalBodyStream);
        }

        private static string FormatHeaders(IHeaderDictionary headers)
        {
            var sb = new StringBuilder();
            foreach (var header in headers)
            {
                sb.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }
            return sb.ToString();
        }

        private static async Task<string> ReadStreamAsync(Stream stream)
        {
            using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            stream.Position = 0; // Сбрасываем позицию потока
            return body;
        }
    }
}
