using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ThaGet.Cqrs.Api.Abstractions;
using ThaGet.Cqrs.Api.EntityFramework.DataSeeders.Enums;
using ThaGet.Cqrs.Domain.DataSeeders;
using ThaGet.Cqrs.Domain.Entities;
using ThaGet.Cqrs.Domain.EntityFramework.Abstractions;

namespace ThaGet.Cqrs.Api.EntityFramework.DataSeeders
{
    public class DataSeederManager<TDbContext, TId>
        where TDbContext : DbContext
        where TId : struct
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDbContextBase _context;

        public DataSeederManager(IServiceProvider serviceProvider, IDbContextBase context)
        {
            _serviceProvider = serviceProvider;
            _context = context;
        }

        public async Task SeedAsync(DataSeederType type)
        {
            // Get set method
            var seederList = GetAllSeeder(type);
            var contextType = _context.GetType();
            var setMethodInfo = contextType.GetMethod(nameof(_context.Set), new Type[0]);

            var constructorArguments = new object[] { _serviceProvider };

            foreach (var seederType in seederList)
            {
                // Create generic set method
                var entityType = seederType.GetInterfaces().First().GetGenericArguments()[0];
                var setMethod = setMethodInfo.MakeGenericMethod(entityType);
                var set = (IQueryable<object>)setMethod.Invoke(_context, null);

                if (!set.Any())
                {
                    // Get seed method
                    var entityInterfaceType = typeof(DataSeederBase<,>).MakeGenericType(entityType);
                    var seedMethod = entityInterfaceType.GetMethod(nameof(DataSeederBase<EntityBase<TId>, TId>.Seed));

                    // Create seeder and call the method on it
                    var seederObject = Activator.CreateInstance(seederType, constructorArguments);
                    seedMethod.Invoke(seederObject, null);
                }
            }

            await _context.SaveAllChangesAsync();
        }

        private IEnumerable<Type> GetAllSeeder(DataSeederType type)
        {
            var assembly = Assembly.GetEntryAssembly();
            var genericType = GetSeederType(type);
            var genericParameterTypes = new Type[] { typeof(EntityBase<TId>) };

            // Get all types implementing the interface {genericType} which generic parameter types match those in {genericParameterTypes}
            var seeders = assembly.GetTypes()
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType &&
                        i.GetGenericTypeDefinition().Equals(genericType) &&
                        i.GetGenericArguments().Count() == genericParameterTypes.Length &&
                        i.GetGenericArguments().Zip(
                            genericParameterTypes,
                            (f, s) => s.IsAssignableFrom(f)
                        ).All(z => z)
                    )
                ).ToList();

            return seeders;
        }

        private Type GetSeederType(DataSeederType type)
        {
            return type switch
            {
                DataSeederType.Core => typeof(ICoreDataSeeder<,>),
                DataSeederType.Test => typeof(ITestDataSeeder<,>),
                _ => throw new ArgumentException($"Invalid type '{type}'", nameof(type)),
            };
        }
    }
}
