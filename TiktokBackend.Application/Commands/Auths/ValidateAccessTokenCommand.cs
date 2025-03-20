using MediatR;
using TiktokBackend.Application.DTOs;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Commands.Auths
{
    public record ValidateAccessTokenCommand(string AccessToken):IRequest<UserTokenDto>;
    public class ValidateAccessTokenCommandHandler : IRequestHandler<ValidateAccessTokenCommand, UserTokenDto>
    {
        private readonly IJwtService _jwtService;

        public ValidateAccessTokenCommandHandler(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public Task<UserTokenDto> Handle(ValidateAccessTokenCommand request, CancellationToken cancellationToken)
        {

            return Task.FromResult(_jwtService.ValidateToken(request.AccessToken));
        }
    }
}
