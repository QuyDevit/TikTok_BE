using Microsoft.Extensions.Caching.Distributed;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Infrastructure.Services
{
    public class OtpCacheService : IOtpCacheService
    {
        private readonly IDistributedCache _cache;
        public OtpCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task<string?> GetAsync(string key)
        {
            try
            {
                return await _cache.GetStringAsync(key);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis GetAsync error: {ex.Message}");
                return null; 
            }
        }

        public async Task SetAsync(string key, string value, int expirationSeconds)
        {
            try
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expirationSeconds)
                };
                await _cache.SetStringAsync(key, value, options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis SetAsync error: {ex.Message}");
            }
            
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _cache.RemoveAsync(key);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis RemoveAsync error: {ex.Message}");
            }
        }
    }
}
