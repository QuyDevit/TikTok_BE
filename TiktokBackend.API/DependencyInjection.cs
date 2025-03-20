using TiktokBackend.Application;
using TiktokBackend.Infrastructure;

namespace TiktokBackend.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationDI(); 

            services.AddInfrastructureDI(configuration);

            return services;
        }
    }
}
