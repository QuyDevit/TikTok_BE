using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("TiktokDBConnection")));
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

            string isRedisEnabledStr = configuration["RedisConfiguration:Enabled"];
            bool isRedisEnabled = !string.IsNullOrEmpty(isRedisEnabledStr) && bool.Parse(isRedisEnabledStr);

            if (isRedisEnabled)
            {
                string connectionString = configuration["RedisConfiguration:ConnectionString"];

                if (!string.IsNullOrEmpty(connectionString))
                {
                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = connectionString;
                        options.InstanceName = "TiktokBackend_";
                    });

                    services.AddSingleton<IRedisService, RedisService>();
                }
            }

            return services;
        }
    }
}
