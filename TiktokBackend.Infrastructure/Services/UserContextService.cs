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
            var userAgent = GetUserAgent();
            var ipAddress = GetIpAddress();
            var acceptLanguage = _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? "Unknown";
            var timeZone = _httpContextAccessor.HttpContext?.Request.Headers["Sec-Ch-Ua-Timezone"].ToString() ?? "Unknown";

            string rawData = $"{userAgent}|{ipAddress}|{acceptLanguage}|{timeZone}";

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}
