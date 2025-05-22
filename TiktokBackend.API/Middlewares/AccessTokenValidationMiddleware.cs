using MediatR;
using TiktokBackend.Application.Commands.Auths;

namespace TiktokBackend.API.Middlewares
{
    public class AccessTokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public AccessTokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ISender sender)
        {
            var accessToken = context.Request.Cookies.TryGetValue("accessToken", out var token)
                ? token
                : null;

            if (!string.IsNullOrEmpty(accessToken))
            {
                var tokenInfo = await sender.Send(new ValidateAccessTokenCommand(accessToken));
                if (tokenInfo != null)
                {
                    context.Items["UserId"] = tokenInfo.UserId;
                    context.Items["Role"] = tokenInfo.Role;
                }
            }

            await _next(context);
        }
    }
}
