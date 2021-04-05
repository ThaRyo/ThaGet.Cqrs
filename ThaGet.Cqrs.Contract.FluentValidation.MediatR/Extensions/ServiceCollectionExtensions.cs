using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ThaGet.Shared.Extensions;

namespace ThaGet.Cqrs.Contract.FluentValidation.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddValidation(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            services.AddWithLifetime(typeof(IPipelineBehavior<,>), typeof(ContractValidationBehavior<,>), lifetime);

            return services;
        }
    }
}
