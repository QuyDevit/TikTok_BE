using MediatR;
using TiktokBackend.Application.Common;
using TiktokBackend.Application.Interfaces;
using TiktokBackend.Application.Payloads;
using TiktokBackend.Application.Validators;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Commands.Auths
{
    public record RegisterWithOtpCommand(RegisterRequest.RequestOtp RequestOtp) : IRequest<ServiceResponse<string>>;
    public class RegisterWithOtpCommandHandler : IRequestHandler<RegisterWithOtpCommand, ServiceResponse<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IOtpCacheService _otpCache;
        private readonly IEmailTemplateService _emailTemplateService;
        public RegisterWithOtpCommandHandler(IOtpCacheService redisService, IUserRepository userRepository
            ,IEmailService emailService, IEmailTemplateService emailTemplateService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _otpCache = redisService;
            _emailTemplateService = emailTemplateService;
        }
        public async Task<ServiceResponse<string>> Handle(RegisterWithOtpCommand request, CancellationToken cancellationToken)
        {
            

            var req = request.RequestOtp;
            var validationResult = RegisterRequestValidator.ValidateOtp(req);
            if (!validationResult.Success)
                return ServiceResponse<string>.Fail(validationResult.Message);

            if (req.Type == "email")
            {
                if (await _userRepository.GetUserByEmailAsync(req.Email) is not null)
                    return ServiceResponse<string>.Fail("Email đã tồn tại. Hãy sử dụng email khác.");
            }  
            if (req.Type == "phone" && await _userRepository.GetUserByPhoneAsync(req.PhoneNumber) is not null)
                return ServiceResponse<string>.Fail("Số điện thoại đã tồn tại. Hãy sử dụng số khác.");

            var otp = new Random().Next(100000, 999999).ToString();
      
            if (req.Type == "email")
            {
                var placeholders = new Dictionary<string, string>
                {
                    { "OTP_CODE", otp }
                };
                var template = await _emailTemplateService.GetEmailTemplateAsync("SendOtpMail", placeholders);
                var isSuccess = await _emailService.SendEmailAsync(req.Email, "Mã xác thực đăng ký tài khoản!", template);
                if (!isSuccess) return ServiceResponse<string>.Fail("Không thể gửi email!");
            }

            string key = $"register:otp:{req.Type}:{req.Email ?? req.PhoneNumber}";
            await _otpCache.SetAsync(key, otp, 300);

            return ServiceResponse<string>.Ok(req.Type == "email" ? "" : otp, "Đã gửi mã xác thực. Vui lòng kiểm tra!");
        }
    }
}
