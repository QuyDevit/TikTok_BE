using TiktokBackend.Application.Interfaces;
using TiktokBackend.Domain.Entities;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IJwtService _jwtService;
        private readonly IRedisService _redisService;
        private readonly IEmailService _emailService;
        public AuthRepository(IJwtService jwtService, IRedisService redisService, IEmailService emailService) {
            _jwtService = jwtService;
            _redisService = redisService;
            _emailService = emailService;
        }
        public async Task<bool> RegisterWithOtpAsync(string email)
        {
            // 1. Tạo mã OTP ngẫu nhiên
            var otp = new Random().Next(100000, 999999).ToString();

            // 2. Lưu OTP vào Redis (hết hạn sau 5 phút)
            await _redisService.SetAsync($"otp:{email}", otp, 300);

            // 3. Gửi email OTP
            await _emailService.SendEmailAsync(email, "Mã OTP của bạn", $"Mã OTP của bạn là: {otp}");

            return true;
        }
    }
}
