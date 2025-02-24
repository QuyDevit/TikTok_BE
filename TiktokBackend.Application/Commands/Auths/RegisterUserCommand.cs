using MediatR;
using TiktokBackend.Application.Common;
using TiktokBackend.Application.Interfaces;
using TiktokBackend.Application.Payloads;
using TiktokBackend.Application.Validators;
using TiktokBackend.Domain.Entities;
using TiktokBackend.Domain.Interfaces;


namespace TiktokBackend.Application.Commands.Auths
{
    public record RegisterUserCommand(RegisterRequest.RegisterUser Register) : IRequest<ServiceResponse<bool>>;
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ServiceResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IRedisService _redisService;
        private readonly IJwtService _jwtService;
        private readonly IUserContextService _userContextService;
        private readonly ICookieService _cookieService;

        public RegisterUserCommandHandler(IUserRepository userRepository, IRedisService redisService,
            IJwtService jwtService,IUserRoleRepository userRoleRepository,IUserTokenRepository userTokenRepository,
            IUserContextService userContextService,IUnitOfWork unitOfWork,ICookieService cookieService)
        {
            _userRepository = userRepository;
            _redisService = redisService;   
            _jwtService = jwtService;
            _userRoleRepository = userRoleRepository;
            _userTokenRepository = userTokenRepository;
            _userContextService = userContextService;
            _unitOfWork = unitOfWork;
            _cookieService = cookieService;
        }

        public async Task<ServiceResponse<bool>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var req = request.Register;
                var validationResult = RegisterRequestValidator.Validate(req);
                if (!validationResult.Success)
                    return ServiceResponse<bool>.Fail(validationResult.Message);
                string key = $"otp:{req.Type}:{req.Email ?? req.PhoneNumber}";
                var storedOtp = await _redisService.GetAsync(key);
                if (string.IsNullOrEmpty(storedOtp) || storedOtp != req.VerificationCode )
                    return ServiceResponse<bool>.Fail("Mã xác thực không chính xác.");
                User newUser;
                if (req.Type == "email")
                {
                    newUser = new User
                    {
                        Email = req.Email,
                        DateOfBirth = req.DateOfBirth,
                        Password = req.Password,
                        EmailVerificationCode = req.VerificationCode
                    };

                }
                else
                {
                    newUser = new User
                    {
                        PhoneNumber = req.PhoneNumber,
                        DateOfBirth = req.DateOfBirth,
                        PhoneVerificationCode = req.VerificationCode
                    };
                }
                var result = await _userRepository.AddUserAsync(newUser);
                if (result == null)
                    return ServiceResponse<bool>.Fail("Đăng ký thất bại.");

                var actoken = _jwtService.GenerateToken(result,"User");
                var rftoken = _jwtService.GenerateRefreshToken();

                await _userRoleRepository.AddOrSkipUserRoleAsync(result.Id);

                string ipAddress = _userContextService.GetIpAddress();
                string userAgent = _userContextService.GetUserAgent();


                await _userTokenRepository.AddOrUpdateUserTokenAsync(result.Id, rftoken, ipAddress, userAgent);

                await _unitOfWork.CommitAsync();

                _cookieService.SetAccessToken(actoken);
                _cookieService.SetRefreshToken(rftoken);

                return ServiceResponse<bool>.Ok(true, "Đăng ký thành công");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync(); 
                return ServiceResponse<bool>.Fail($"Đăng ký thất bại: {ex.Message}");
            }
        }
    }
}
