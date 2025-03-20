using TiktokBackend.Domain.Entities;

namespace TiktokBackend.Domain.Interfaces
{
    public interface IApiKeyRepository
    {
        Task<ApiKey?> AddAsync(Guid userid);
        Task<bool> GetByKeyAsync(string key);
    }
}
