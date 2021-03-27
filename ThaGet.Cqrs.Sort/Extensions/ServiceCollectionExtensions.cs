using System.Linq;
using System.Reflection;
using ThaGet.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using ThaGet.Cqrs.Sort.Abstractions;

namespace ThaGet.Cqrs.Sort.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSort<TId>(this IServiceCollection services, Assembly assembly, ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TId : struct
        {
            // Add service
            services.AddWithLifetime<ISortService<TId>, SortService<TId>>(lifetime);

            // Gather valid types from assembly
            var types = assembly.GetTypes();

            SortService<TId>.SortTypeList = types
                .Where(type => type.IsClass
                    && !type.IsAbstract
                    && type.IsAssignableTo(typeof(AbstractSort<,,>))
                ).ToList();

            return services;
        }
    }
}
