using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ThaGet.Shared;

namespace ThaGet.Cqrs.Domain.EntityFramework.Extensions
{

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppDbContext<TDbContext>(this IServiceCollection services, Action<DbContextConfigOptions> configOptions)
            where TDbContext : DbContext
        {
            var dbOptions = new DbContextConfigOptions();
            configOptions?.Invoke(dbOptions);

            ArgumentHelper.ThrowIfNullOrEmpty(dbOptions.EntitySchema, nameof(dbOptions.EntitySchema), "No schema provided");
            ArgumentHelper.ThrowIfNullOrEmpty(dbOptions.ConnectionString, nameof(dbOptions.ConnectionString), "No connection string supplied");

            services.AddDbContext<TDbContext>(options =>
                options.UseSqlServer(dbOptions.ConnectionString, x => x.MigrationsHistoryTable("_EFMigrationsHistory", dbOptions.EntitySchema))
                    .EnableSensitiveDataLogging(dbOptions.IsSensitiveDataLoggingEnabled));

            // TODO Check what this does
            // .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)));

            return services;
        }
    }
}
