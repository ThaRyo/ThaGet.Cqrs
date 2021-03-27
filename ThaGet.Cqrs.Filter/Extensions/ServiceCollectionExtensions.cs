using System.Linq;
using System.Reflection;
using ThaGet.Shared.Extensions;
using ThaGet.Cqrs.Filter.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ThaGet.Cqrs.Filter.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFilter<TId>(this IServiceCollection services, Assembly assembly, ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TId : struct
        {
            // Add service
            services.AddWithLifetime<IFilterService<TId>, FilterService<TId>>(lifetime);
            
            // Gather valid types from assembly
            var types = assembly.GetTypes();

            FilterService<TId>.FilterTypeList = types
                .Where(type => type.IsClass
                    && !type.IsAbstract
                    && type.IsAssignableTo(typeof(AbstractFilter<,,>))
                ).ToList();

            return services;
        }
    }
}