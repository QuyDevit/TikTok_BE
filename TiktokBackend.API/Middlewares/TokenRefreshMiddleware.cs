using MediatR;
using TiktokBackend.Application.Commands.Auths;
using TiktokBackend.Application.DTOs;

namespace TiktokBackend.API.Middlewares
{
    public class TokenRefreshMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenRefreshMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context, ISender sender)
        {
            var acToken = GetCookie(context, "accessToken");
            var rfToken = GetCookie(context, "refreshToken");

            if (string.IsNullOrEmpty(acToken) && string.IsNullOrEmpty(rfToken))
            {
                await _next(context);
                return;
            }

            var tokenInfo = await ValidateAndRefreshToken(sender, acToken, rfToken);

            if (tokenInfo != null)
            {
                await AuthenticateUser(context, tokenInfo);
                return;
            }
            ClearCookiesAndDenyAccess(context);
        }

        private static string GetCookie(HttpContext context, string key) =>
            context.Request.Cookies.TryGetValue(key, out var value) ? value : null;

        private async Task<UserTokenDto?> ValidateAndRefreshToken(ISender sender, string? acToken, string? rfToken)
        {
            var tokenInfo = !string.IsNullOrEmpty(acToken)
                ? await sender.Send(new ValidateAccessTokenCommand(acToken))
                : null;

            if (tokenInfo == null && !string.IsNullOrEmpty(rfToken))
            {
                var validRefreshToken = await sender.Send(new ValidateRefreshTokenCommand(rfToken));
                if (validRefreshToken != null)
                {
                    await sender.Send(new SetCookieCommand(validRefreshToken.AccessToken, validRefreshToken.RefreshToken));
                    return new UserTokenDto
                    {
                        UserId = validRefreshToken.UserId,
                        Role = validRefreshToken.Role,
                        RefreshToken = validRefreshToken.RefreshToken, 
                    };
                }
            }

            return tokenInfo;
        }

        private async Task AuthenticateUser(HttpContext context, UserTokenDto tokenInfo)
        {
            context.Items["UserId"] = tokenInfo.UserId;
            context.Items["Role"] = tokenInfo.Role;
            if(context.Request.Path.Value?.Contains("/logout",StringComparison.OrdinalIgnoreCase) == true)
            {
                context.Items["RefreshToken"] = tokenInfo.RefreshToken;
            }
            await _next(context);
        }

        private static void ClearCookiesAndDenyAccess(HttpContext context)
        {
            context.Response.Cookies.Delete("accessToken");
            context.Response.Cookies.Delete("refreshToken");
            context.Response.StatusCode = 401;
        }
    }
}
