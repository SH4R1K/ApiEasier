using ApiEasier.Logger.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ApiEasier.Bll.Services.Logger
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
            context.Request.EnableBuffering();

            using (var reader = new StreamReader(
                context.Request.Body, leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            using (var buffer = new MemoryStream())
            {
                var stream = context.Response.Body;
                context.Response.Body = buffer;

                await _next(context);

                buffer.Seek(0, SeekOrigin.Begin);
                using (var bufferReader = new StreamReader(buffer))
                {
                    responseBody = await bufferReader.ReadToEndAsync();
                    buffer.Seek(0, SeekOrigin.Begin);
                    await buffer.CopyToAsync(stream);
                    context.Response.Body = stream;
                }
                _logger.LogHttp(context, requestBody, responseBody);
            }
        }
    }
}
