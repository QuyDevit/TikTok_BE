using MediatR;
using System.Text.Json;
using TiktokBackend.Application.Queries.ApiKeys;

namespace TiktokBackend.API.Middlewares
{
    public class ApiKeyAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context,ISender sender, IConfiguration configuration)
        {
            var apiKeyHeader = configuration["ApiConfig:ApiHeader"];
            var apiKeyConfig = configuration["ApiConfig:ApiKey"];
            if (!context.Request.Headers.TryGetValue(apiKeyHeader, out var apiKey))
            {
                await WriteJsonResponse(context, StatusCodes.Status401Unauthorized, "Thiếu Api Key.");
                return;
            }
            if(apiKey == apiKeyConfig)
            {
                await _next(context);
                return;
            }
            var isValid = await sender.Send(new ValidateApiKeyQuery(apiKey));
            if (!isValid)
            {
                await WriteJsonResponse(context, StatusCodes.Status403Forbidden, "Api Key không tồn tại hoặc đã hết hạn.");
                return;
            }

            await _next(context);
        }
        private static async Task WriteJsonResponse(HttpContext context, int statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var json = JsonSerializer.Serialize(new { success = false, message });
            await context.Response.WriteAsync(json);
        }
    }
}
