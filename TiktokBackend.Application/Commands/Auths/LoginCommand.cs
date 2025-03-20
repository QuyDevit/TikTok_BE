using MediatR;
using TiktokBackend.Application.Common;
using TiktokBackend.Application.Interfaces;
using TiktokBackend.Application.Validators;
using TiktokBackend.Domain.Entities;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Commands.Auths
{
    /// <summary>
    /// Lệnh đăng nhập, có thể sử dụng mật khẩu hoặc mã OTP.
    /// Nếu Type == "phone" thì UserName = PhoneNumber, nếu Type == "email" thì UserName = UserName
    /// Nếu Type == "phone" và IsOtp == true thì OTP(Password = OTP) ngược lại Password = Password, nếu Type == "email" thì sử dụng mật khẩu(Password = Password). 
    /// </summary>
    public record LoginCommand(string Type,string UserName,string Password,bool IsOtp = false): IRequest<ServiceResponse<bool>>;
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ServiceResponse<bool>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOtpCacheService _redisService;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IUserContextService _userContextService;
        private readonly ICookieService _cookieService;

        public LoginCommandHandler(IUserRepository userRepository,IOtpCacheService redisService,IJwtService jwtService,
            IUnitOfWork unitOfWork, IUserRoleRepository userRoleRepository, IUserTokenRepository userTokenRepository,
            IUserContextService userContextService, ICookieService cookieService)
        {
            _userRepository = userRepository;
            _redisService = redisService;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
            _userRoleRepository = userRoleRepository;
            _userTokenRepository = userTokenRepository;
            _userContextService = userContextService;
            _cookieService = cookieService;
        }

        public async Task<ServiceResponse<bool>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                User user;
                var validationResult = LoginRequestValidator.Validate(request);
                if (!validationResult.Success)
                    return ServiceResponse<bool>.Fail(validationResult.Message);

                if (request.Type == "phone")
                {
                    if (request.IsOtp)
                    {
                        string key = $"login:otp:{request.UserName}";
                        var storedOtp = await _redisService.GetAsync(key);
                        if (string.IsNullOrEmpty(storedOtp) || storedOtp != request.Password)
                            return ServiceResponse<bool>.Fail("Mã xác thực không chính xác.");

                        await _redisService.RemoveAsync(key);
                        user = await _userRepository.GetUserByPhoneAsync(request.UserName);
                    }
                    else
                    {
                        user = await _userRepository.ValidateUserCasePhoneAsync(request.UserName,request.Password);
                        if (user is null)
                            return ServiceResponse<bool>.Fail("Mật khẩu hoặc tài khoản không chính xác!");
                    }                 
                }
                else
                {
                    user = await _userRepository.ValidateUserCaseEmailAsync(request.UserName, request.Password);
                    if (user is null) 
                        return ServiceResponse<bool>.Fail("Mật khẩu hoặc tài khoản không chính xác!");
                }
                var actoken = _jwtService.GenerateToken(user, "User");
                var rftoken = _jwtService.GenerateRefreshToken();
                await _userRoleRepository.AddOrSkipUserRoleAsync(user.Id);

                string ipAddress = _userContextService.GetIpAddress();
                string userAgent = _userContextService.GetUserAgent();
                string deviceId = _userContextService.GetDeviceId();

                await _userTokenRepository.AddOrUpdateUserTokenAsync(user.Id, rftoken, ipAddress, userAgent,deviceId);
                await _unitOfWork.CommitAsync();

                _cookieService.SetAccessToken(actoken);
                _cookieService.SetRefreshToken(rftoken);
                return ServiceResponse<bool>.Ok(true, "Đăng nhập thành công");
            }
            catch (Exception ) {
                await _unitOfWork.RollbackAsync();
                return ServiceResponse<bool>.Fail("Đăng nhập thất bại");
            } 
        }
    }
}
