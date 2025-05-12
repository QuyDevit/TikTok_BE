using MediatR;
using TiktokBackend.Application.Common;
using TiktokBackend.Application.Interfaces;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Commands.Auths
{
    public record LogoutCommand(Guid UserId) : IRequest<ServiceResponse<bool>>;

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ServiceResponse<bool>>
    {
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly ICookieService _cookieService;
        private readonly IUnitOfWork _unitOfWork;
        public LogoutCommandHandler(IUserTokenRepository userTokenRepository, ICookieService cookieService,IUnitOfWork unitOfWork)
        {
            _userTokenRepository = userTokenRepository;
            _cookieService = cookieService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = _cookieService.GetRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
                return ServiceResponse<bool>.Fail("Đăng xuất thất bại!");
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await _userTokenRepository.RemoveRefreshTokenAsync(request.UserId, refreshToken);
                if (!result)
                    return ServiceResponse<bool>.Fail("Đăng xuất thất bại!");
                await _unitOfWork.CommitAsync();

                _cookieService.ClearCookie();

                return ServiceResponse<bool>.Ok(true, "Đăng xuất thành công!");
            }
            catch (Exception ) { 
                 await _unitOfWork.RollbackAsync();
                return ServiceResponse<bool>.Fail("Đăng xuất thất bại!");
            } 
        }
    }
}
