using MediatR;
using TiktokBackend.Application.Common;
using TiktokBackend.Application.Interfaces;
using TiktokBackend.Application.Payloads;
using TiktokBackend.Application.Validators;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Commands.Auths
{
    public record RegisterWithOtpCommand(RegisterRequest.RequestOtp RequestOtp) : IRequest<ServiceResponse<bool>>;
    public class RegisterWithOtpCommandHandler : IRequestHandler<RegisterWithOtpCommand, ServiceResponse<bool>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IRedisService _redisService;
        public RegisterWithOtpCommandHandler(IRedisService redisService, IUserRepository userRepository,IEmailService emailService) {
            _userRepository = userRepository;
            _emailService = emailService;
            _redisService = redisService;
        }
        public async Task<ServiceResponse<bool>> Handle(RegisterWithOtpCommand request, CancellationToken cancellationToken)
        {
            var req = request.RequestOtp;
            var validationResult = RegisterRequestValidator.ValidateOtp(req);
            if (!validationResult.Success)
                return ServiceResponse<bool>.Fail(validationResult.Message);

            if (req.Type == "email")
            {
                if (await _userRepository.GetUserByEmailAsync(req.Email) is not null)
                    return ServiceResponse<bool>.Fail("Email đã tồn tại. Hãy sử dụng email khác.");
            }  
            if (req.Type == "phone" && await _userRepository.GetUserByPhoneAsync(req.PhoneNumber) is not null)
                return ServiceResponse<bool>.Fail("Số điện thoại đã tồn tại. Hãy sử dụng số khác.");

            var otp = new Random().Next(100000, 999999).ToString();
      
            if (req.Type == "email")
            {
                var isSuccess = await _emailService.SendEmailAsync(req.Email, "Mã xác thực đăng ký tài khoản!", otp);
                if (!isSuccess) return ServiceResponse<bool>.Fail("Không thể gửi email!");
            }

            string key = $"otp:{req.Type}:{req.Email ?? req.PhoneNumber}";
            await _redisService.SetAsync(key, otp, 300);

            return ServiceResponse<bool>.Ok(true, "Đã gửi mã xác thực. Vui lòng kiểm tra!");
        }
    }
}
