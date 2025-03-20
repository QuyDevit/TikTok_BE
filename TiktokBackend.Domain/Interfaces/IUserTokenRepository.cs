using TiktokBackend.Domain.Entities;

namespace TiktokBackend.Domain.Interfaces
{
    public interface IUserTokenRepository
    {
        Task AddOrUpdateUserTokenAsync(Guid userId,string rfToken,string ipAddress,string userAgent,string deviceId);
        Task<UserToken?> RefreshTokenAsync(string token,string deviceId);
        Task<bool> RemoveRefreshTokenAsync(Guid userId,string token);
    }
}
