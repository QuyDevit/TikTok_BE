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
            var data = await _cache.GetAsync(key);
            return data == null ? null : Encoding.UTF8.GetString(data);
        }

        public async Task SetAsync(string key, string value, int expirationSeconds)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expirationSeconds)
            };
            await _cache.SetAsync(key, Encoding.UTF8.GetBytes(value), options);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
