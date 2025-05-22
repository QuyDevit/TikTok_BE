using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using TiktokBackend.Application.Interfaces;

namespace TiktokBackend.Infrastructure.Services
{
    public class VideoLikeCacheService : IVideoLikeCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IDatabase _redis;
        public VideoLikeCacheService(IDistributedCache cache, IConnectionMultiplexer redisConnection)
        {
            _cache = cache;
            _redis = redisConnection.GetDatabase();
        }
        private const string VideoIdSetKey = "video:like:ids";
        private string GetCacheKey(Guid videoId) => $"video:like:{videoId}";

        public async Task<int> IncrementLikeAsync(Guid videoId, Func<Task<int>> getFromDbFallback)
        {
            var key = GetCacheKey(videoId);
            var countStr = await _cache.GetStringAsync(key);

            int count = string.IsNullOrEmpty(countStr) ? await getFromDbFallback() : int.Parse(countStr);
            count++;

            await _cache.SetStringAsync(key, count.ToString());

            await _redis.SetAddAsync(VideoIdSetKey, videoId.ToString());

            return count;
        }

        public async Task<int> DecrementLikeAsync(Guid videoId, Func<Task<int>> getFromDbFallback)
        {
            var key = GetCacheKey(videoId);
            var countStr = await _cache.GetStringAsync(key);

            int count = string.IsNullOrEmpty(countStr) ? await getFromDbFallback() : int.Parse(countStr);
            count = Math.Max(0, count - 1);

            await _cache.SetStringAsync(key, count.ToString());

            await _redis.SetAddAsync(VideoIdSetKey, videoId.ToString());

            return count;
        }

        public async Task<int?> GetLikeCountAsync(Guid videoId)
        {
            var key = GetCacheKey(videoId);
            var countStr = await _cache.GetStringAsync(key);
            return string.IsNullOrEmpty(countStr) ? null : int.Parse(countStr);
        }

        public async Task ResetLikeCountAsync(Guid videoId)
        {
            await _cache.RemoveAsync(GetCacheKey(videoId));
        }

        public async Task<List<Guid>> GetAllCachedVideoIdsAsync()
        {
            var members = await _redis.SetMembersAsync(VideoIdSetKey);
            return members.Select(m => Guid.Parse(m.ToString())).ToList();
        }
    }
}
