using TiktokBackend.Application.DTOs;

namespace TiktokBackend.Application.Interfaces
{
    public interface IUserCacheService
    {
        Task SetUserAsync(Guid id, UserDto user, int expirationSeconds);
        Task<UserDto?> GetUserAsync(Guid id);
        Task RemoveUserAsync(Guid id);
    }
}
