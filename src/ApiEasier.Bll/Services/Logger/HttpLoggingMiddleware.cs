using ApiEasier.Bll.Interfaces.Logger;
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
            using (StreamReader stream = new StreamReader(context.Request.Body))
            {
                requestBody = await stream.ReadToEndAsync();
            }

            using (var buffer = new MemoryStream())
            {
                var stream = context.Response.Body;
                context.Response.Body = buffer;

                await _next(context);

                buffer.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(buffer);
                using (var bufferReader = new StreamReader(buffer))
                {
                    string body = await bufferReader.ReadToEndAsync();
                    buffer.Seek(0, SeekOrigin.Begin);
                    await buffer.CopyToAsync(stream);
                    context.Response.Body = stream;
                    _logger.LogHttp(context, requestBody, body);
                }
            }
        }
    }
}
