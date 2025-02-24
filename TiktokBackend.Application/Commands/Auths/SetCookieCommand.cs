using MediatR;
using TiktokBackend.Application.Interfaces;

namespace TiktokBackend.Application.Commands.Auths
{
    public record SetCookieCommand(string AccessToken,string RefreshToken): IRequest<Unit>;
    public class SetCookieCommandHandler : IRequestHandler<SetCookieCommand, Unit>
    {
        private readonly ICookieService _cookieService;

        public SetCookieCommandHandler(ICookieService cookieService)
        {
            _cookieService = cookieService;
        }
        public Task<Unit> Handle(SetCookieCommand request, CancellationToken cancellationToken)
        {
            _cookieService.SetAccessToken(request.AccessToken);
            _cookieService.SetRefreshToken(request.RefreshToken);

            return Task.FromResult(Unit.Value);
        }
    }
}
