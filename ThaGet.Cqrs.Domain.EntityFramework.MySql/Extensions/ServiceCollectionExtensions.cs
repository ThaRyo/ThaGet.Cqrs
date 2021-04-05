using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ThaGet.Cqrs.Domain.EntityFramework.MySql.Extensions
{

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppDbContext<TDbContext>(this IServiceCollection services, Action<DatabaseConfigOptions> configOptions)
            where TDbContext : DbContext
        {
            var dbOptions = new DatabaseConfigOptions();
            configOptions?.Invoke(dbOptions);

            if(!dbOptions.IsValid())
                throw new ArgumentException("Database configuration is invalid");

            services.AddDbContext<TDbContext>(options =>
                options.UseMySQL(
                    dbOptions.BuildConnectionString()
                    // TODO check if mysql support that
                    //x => x.MigrationsHistoryTable("_EFMigrationsHistory", dbOptions.EntitySchema)
                )
            );

            return services;
        }
    }
}
