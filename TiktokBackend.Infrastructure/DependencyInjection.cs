using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using StackExchange.Redis;
using TiktokBackend.Application.Interfaces;
using TiktokBackend.Domain.Interfaces;
using TiktokBackend.Infrastructure.Persistence;
using TiktokBackend.Infrastructure.Repositories;
using TiktokBackend.Infrastructure.Services;

namespace TiktokBackend.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<AppDbContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("TiktokDBConnection"))
            );

            // Repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IApiKeyRepository, ApiKeyRepository>();      
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();
            services.AddScoped<IVideoRepository, VideoRepository>();
            services.AddScoped<IVideoMetaRepository, VideoMetaRepository>();
            services.AddScoped<IFollowRepository, FollowRepository>();
            services.AddScoped<IVideoLikeRepository, VideoLikeRepository>();

            // Services
            services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddSingleton<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<ICookieService, CookieService>();
            services.AddScoped<IUploadFileService, UploadFileService>();
            services.AddScoped<IVideoMetaService, VideoMetaService>();
            services.AddScoped<IUserSearchService, UserSearchService>();
            services.AddScoped<IVideoSearchService, VideoSearchService>();
            services.AddScoped<DataSyncService>();

            // Background jobs
            services.AddHostedService<ElasticsearchSyncHostedService>();
            services.AddHostedService<LikeCountSyncWorker>();

            string isRedisEnabledStr = configuration["RedisConfiguration:Enabled"];
            bool isRedisEnabled = !string.IsNullOrEmpty(isRedisEnabledStr) && bool.Parse(isRedisEnabledStr);

            if (!isRedisEnabled)
                return services;
            string connectionString = configuration["RedisConfiguration:ConnectionString"];

            if (!string.IsNullOrEmpty(connectionString))
            {              
                // Đăng ký IDistributedCache
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = connectionString;
                    options.InstanceName = "TiktokBackend_";
                });
                // Đăng ký ConnectionMultiplexer dùng cho StackExchange.Redis
                services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(connectionString));

                services.AddSingleton<IOtpCacheService, OtpCacheService>();
                services.AddSingleton<IUserCacheService, UserCacheService>();
                services.AddScoped<IVideoLikeCacheService, VideoLikeCacheService>();
            }
            // Elasticsearch
            string elasticSearchUri = configuration["Elasticsearch:Uri"];
            if (!string.IsNullOrEmpty(elasticSearchUri))
            {
                var settings = new ConnectionSettings(new Uri(elasticSearchUri));
                var client = new ElasticClient(settings);
                services.AddSingleton<IElasticClient>(client);
            }
            return services;
        }
    }
}
