using System;
using ThaGet.Cqrs.Api.Abstractions;
using ThaGet.Cqrs.Domain.Entities.Abstractions;

namespace ThaGet.Cqrs.Api.DataSeeders
{
    public abstract class DataSeederBase<TEntity, TId> : IDataSeeder<TEntity, TId>
        where TEntity : IEntity<TId>
        where TId : struct
    {
        protected IServiceProvider ServiceProvider { get; }

        public DataSeederBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public abstract void Seed();
    }
}
