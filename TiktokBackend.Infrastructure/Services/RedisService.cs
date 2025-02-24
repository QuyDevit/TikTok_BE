using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Infrastructure.Services
{
    public class RedisService : IRedisService
    {
        private readonly IDistributedCache _cache;
        public RedisService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task<string?> GetAsync(string key)
        {
            try
            {
                var data = await _cache.GetAsync(key);
                return data == null ? null : Encoding.UTF8.GetString(data);
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
                await _cache.SetAsync(key, Encoding.UTF8.GetBytes(value), options);
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
