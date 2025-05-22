using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TiktokBackend.Application.Interfaces;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Infrastructure.Services
{
    public class LikeCountSyncWorker :BackgroundService
    {
        private readonly ILogger<LikeCountSyncWorker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public LikeCountSyncWorker(
        ILogger<LikeCountSyncWorker> logger,
        IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("⏳ [LikeCountSyncWorker] Starting sync like count from Redis to DB...");
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var videoLikeCache = scope.ServiceProvider.GetRequiredService<IVideoLikeCacheService>();
                    var videoRepository = scope.ServiceProvider.GetRequiredService<IVideoRepository>();
                    var videoSearchService = scope.ServiceProvider.GetRequiredService<IVideoSearchService>();

                    var videoIds = await videoLikeCache.GetAllCachedVideoIdsAsync();

                    foreach (var videoId in videoIds)
                    {
                        var likeCount = await videoLikeCache.GetLikeCountAsync(videoId);
                        if (likeCount.HasValue)
                        {
                            await videoRepository.UpdateLikeCountAsync(videoId, likeCount.Value);
                            await videoSearchService.UpdateLikesCountAsync(videoId, likeCount.Value);

                            _logger.LogInformation($"✅ Synced likes for video {videoId} = {likeCount.Value}");
                        }
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }
}
