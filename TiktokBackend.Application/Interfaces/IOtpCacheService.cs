namespace TiktokBackend.Domain.Interfaces
{
    public interface IOtpCacheService
    {
        Task SetAsync(string key, string value, int expirationSeconds);
        Task<string?> GetAsync(string key);
        Task RemoveAsync(string key);
    }
}
