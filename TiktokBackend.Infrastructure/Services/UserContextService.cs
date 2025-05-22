using TiktokBackend.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Security.Cryptography;

namespace TiktokBackend.Infrastructure.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetIpAddress()
        {
            return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }

        public string GetUserAgent()
        {
            return _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString() ?? "Unknown";
        }
        public string GetDeviceId()
        {
            var context = _httpContextAccessor.HttpContext;
            var cookieKey = "deviceId";

            if (context.Request.Cookies.TryGetValue(cookieKey, out var existingDeviceId))
                return existingDeviceId;

            var newDeviceId = Guid.NewGuid().ToString("N");
            context.Response.Cookies.Append(cookieKey, newDeviceId, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddYears(1), 
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return newDeviceId;
        }

    }
}
