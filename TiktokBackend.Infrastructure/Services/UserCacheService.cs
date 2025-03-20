using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TiktokBackend.Application.DTOs;
using TiktokBackend.Application.Interfaces;

namespace TiktokBackend.Infrastructure.Services
{
    public class UserCacheService : IUserCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

        public UserCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SetUserAsync(Guid id, UserDto user, int expirationSeconds)
        {
            try
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expirationSeconds),
                };

                var userJson = JsonSerializer.Serialize(user, _jsonOptions);
                await _cache.SetStringAsync($"user:{id}", userJson, options);
            }
            catch (Exception ex) {
                Console.WriteLine($"Redis SetUserAsync error: {ex.Message}");
            }
        }
        public async Task<UserDto?> GetUserAsync(Guid id)
        {
            try
            {
                var userJson = await _cache.GetStringAsync($"user:{id}");
                return string.IsNullOrEmpty(userJson) ? null : JsonSerializer.Deserialize<UserDto>(userJson, _jsonOptions);
            }
            catch (Exception ex) {
                Console.WriteLine($"Redis GetUserAsync error: {ex.Message}");
                return null;
            }
        }

        public async Task RemoveUserAsync(Guid id)
        {
            try
            {
                await _cache.RemoveAsync($"user:{id}");
            }
            catch (Exception ex) {
                Console.WriteLine($"Redis RemoveUserAsync error: {ex.Message}");
            }
        }
    }
}
