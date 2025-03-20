using TiktokBackend.Application.DTOs;
using TiktokBackend.Domain.Entities;

namespace TiktokBackend.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user,string role);
        string GenerateRefreshToken();
        UserTokenDto? ValidateToken(string actoken);
    }
}
