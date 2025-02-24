using MediatR;
using TiktokBackend.Application.DTOs;
using TiktokBackend.Application.Interfaces;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Commands.Auths
{
    public record ValidateRefreshTokenCommand(string RefreshToken) : IRequest<UserTokenDto?>;
    public class ValidateRefreshTokenCommandHandler : IRequestHandler<ValidateRefreshTokenCommand, UserTokenDto?>
    {
        private readonly IJwtService _jwtService;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ValidateRefreshTokenCommandHandler(IJwtService jwtService, IUserTokenRepository userTokenRepository
            ,IUserRepository userRepository,IUnitOfWork unitOfWork)
        {
            _jwtService = jwtService;
            _userTokenRepository = userTokenRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserTokenDto?> Handle(ValidateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var validRefreshToken = await _userTokenRepository.RefreshTokenAsync(request.RefreshToken);
                if (validRefreshToken == null || !validRefreshToken.UserId.HasValue)
                    return null;
                var userCurrent = await _userRepository.GetUserByIdAsync(validRefreshToken.UserId.Value);
                if (userCurrent == null)
                    return null;
                var acTokenNew = _jwtService.GenerateToken(userCurrent, "User");
                await _unitOfWork.CommitAsync();
                return new UserTokenDto
                {
                    AccessToken = acTokenNew,
                    RefreshToken = validRefreshToken.RefreshToken,
                    UserId = userCurrent.Id,
                    Role = "User"
                };

            }
            catch (Exception ex) { 
                return null;
            }
        }
    }
}
