using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TiktokBackend.Infrastructure.Services
{
    public class ElasticsearchSyncHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public ElasticsearchSyncHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var syncService = scope.ServiceProvider.GetRequiredService<DataSyncService>();
                await syncService.SyncUsersAsync();
                await syncService.SyncVideosAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
