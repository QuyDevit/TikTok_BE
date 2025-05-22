using Microsoft.AspNetCore.Http;
using Nest;
using TiktokBackend.Application.Interfaces;

namespace TiktokBackend.Infrastructure.Services
{
    public class CookieService : ICookieService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public CookieService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void ClearCookie()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete("accessToken");
            _contextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");
            _contextAccessor.HttpContext?.Response.Cookies.Delete("deviceId");
        }

        public string? GetRefreshToken()
        {
            var newRefreshToken = _contextAccessor.HttpContext.Items["RefreshToken"] as string;
            if (!string.IsNullOrEmpty(newRefreshToken)) 
                return newRefreshToken;
            
            return _contextAccessor.HttpContext?.Request.Cookies.TryGetValue("refreshToken",out var refreshToken) == true 
                ? refreshToken : null;
        }

        public void SetAccessToken(string token)
        {

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(1)
            };
            _contextAccessor.HttpContext?.Response.Cookies.Append("accessToken", token, cookieOptions);
        }

        public void SetRefreshToken(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            _contextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", token, cookieOptions);

        }
    }
}
