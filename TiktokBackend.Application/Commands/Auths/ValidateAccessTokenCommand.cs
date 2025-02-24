using MediatR;
using TiktokBackend.Application.DTOs;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Commands.Auths
{
    public record ValidateAccessTokenCommand(string AccessToken):IRequest<TokenInfoDto>;
    public class ValidateAccessTokenCommandHandler : IRequestHandler<ValidateAccessTokenCommand, TokenInfoDto>
    {
        private readonly IJwtService _jwtService;

        public ValidateAccessTokenCommandHandler(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public Task<TokenInfoDto> Handle(ValidateAccessTokenCommand request, CancellationToken cancellationToken)
        {

            return Task.FromResult(_jwtService.ValidateToken(request.AccessToken));
        }
    }
}
