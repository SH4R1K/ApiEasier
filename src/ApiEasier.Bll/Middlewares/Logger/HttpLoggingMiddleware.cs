using ApiEasier.Logger.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ApiEasier.Bll.Middleware.Logger
{
    public class HttpLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerService _logger;

        public HttpLoggingMiddleware(RequestDelegate next, ILoggerService logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            string requestBody, responseBody;
            context.Request.EnableBuffering(); // Включаем буфферизацию, чтобы можно было прочитать тело запроса без потери данных

            using (var reader = new StreamReader(
                context.Request.Body, leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync(); // Сохраняем тело запроса в строку
                context.Request.Body.Position = 0; // Возвращаем на начало, чтобы можно было опять прочитать
            }

            using (var buffer = new MemoryStream())
            {
                var stream = context.Response.Body; // Сохраняем поток тела ответа
                context.Response.Body = buffer; // Подменяем на свой поток, для последующего чтения ответа

                await _next(context); // Продолжаем работу остальных middleware, чтобы получить тело ответа

                buffer.Seek(0, SeekOrigin.Begin); // Возвращаем на начало
                using (var bufferReader = new StreamReader(buffer))
                {
                    responseBody = await bufferReader.ReadToEndAsync(); // Сохраняем тело ответа в строку
                    buffer.Seek(0, SeekOrigin.Begin);
                    await buffer.CopyToAsync(stream); 
                    context.Response.Body = stream; // Возвращаем тело ответа в контекст
                }
                _logger.LogHttp(context, requestBody, responseBody);
            }
        }
    }
}
