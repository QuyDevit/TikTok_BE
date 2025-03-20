using MediatR;
using TiktokBackend.Application.Common;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Commands.Auths
{
    public record LoginWithOtpCommand(string PhoneNumber):IRequest<ServiceResponse<string>>;
    public class LoginWithOtpCommandHandler : IRequestHandler<LoginWithOtpCommand, ServiceResponse<string>>
    {
        private readonly IOtpCacheService _otpCache;
        private readonly IUserRepository _userRepository;

        public LoginWithOtpCommandHandler(IOtpCacheService otpCache, IUserRepository userRepository)
        {
            _otpCache = otpCache;
            _userRepository = userRepository;
        }

        public async Task<ServiceResponse<string>> Handle(LoginWithOtpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetUserByPhoneAsync(request.PhoneNumber);
                if(user is null)
                    return ServiceResponse<string>.Fail("Số điện thoại không tồn tại!");

                var otp = new Random().Next(100000, 999999).ToString();
                string key = $"login:otp:{request.PhoneNumber}";
                await _otpCache.SetAsync(key, otp, 300);

                return ServiceResponse<string>.Ok(otp, "Đã gửi mã xác thực. Vui lòng kiểm tra!");
            }
            catch (Exception ) {
                return ServiceResponse<string>.Fail("Gửi mã xác thực thất bại!");
            }
        }
    }
}
