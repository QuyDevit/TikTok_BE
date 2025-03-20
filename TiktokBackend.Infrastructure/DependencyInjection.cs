using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
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

            services.AddDbContext<AppDbContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("TiktokDBConnection"))
            );
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IApiKeyRepository, ApiKeyRepository>();      
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();


            services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();

            services.AddScoped<IJwtService, JwtService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddSingleton<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<ICookieService, CookieService>();
            services.AddScoped<IUploadFileService, UploadFileService>();
            services.AddScoped<IUserSearchService, UserSearchService>();
            services.AddScoped<UserSyncService>();
            services.AddHostedService<ElasticsearchSyncHostedService>();

            string isRedisEnabledStr = configuration["RedisConfiguration:Enabled"];
            bool isRedisEnabled = !string.IsNullOrEmpty(isRedisEnabledStr) && bool.Parse(isRedisEnabledStr);

            if (!isRedisEnabled)
                return services;
            string connectionString = configuration["RedisConfiguration:ConnectionString"];

            if (!string.IsNullOrEmpty(connectionString))
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = connectionString;
                    options.InstanceName = "TiktokBackend_";
                });

                services.AddSingleton<IOtpCacheService, OtpCacheService>();
                services.AddSingleton<IUserCacheService, UserCacheService>();
            }

            string elasticSearchUri = configuration["Elasticsearch:Uri"];
            if (!string.IsNullOrEmpty(connectionString))
            {
                var settings = new ConnectionSettings(new Uri(elasticSearchUri))
                    .DefaultIndex("users");
                var client = new ElasticClient(settings);
                services.AddSingleton<IElasticClient>(client);
            }
            return services;
        }
    }
}
