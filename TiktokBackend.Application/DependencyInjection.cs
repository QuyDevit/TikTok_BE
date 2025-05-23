﻿using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TiktokBackend.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
