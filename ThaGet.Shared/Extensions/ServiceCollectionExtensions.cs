using Microsoft.Extensions.DependencyInjection;
using System;

namespace ThaGet.Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWithLifetime<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
            where TService : class
            where TImplementation : class, TService
        {
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton<TService, TImplementation>();
                    break;

                case ServiceLifetime.Scoped:
                    services.AddScoped<TService, TImplementation>();
                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient<TService, TImplementation>();
                    break;
            }

            return services;
        }

        public static IServiceCollection AddWithLifetime(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime)
        {
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton(serviceType, implementationType);
                    break;

                case ServiceLifetime.Scoped:
                    services.AddScoped(serviceType, implementationType);
                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient(serviceType, implementationType);
                    break;
            }

            return services;
        }
    }
}
