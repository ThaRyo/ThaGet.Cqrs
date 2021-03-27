using System;
using System.Linq.Expressions;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Sort.Abstractions;

namespace ThaGet.Cqrs.Sort
{
    internal class SortDefinition<TEntity, TId> : ISortDefinition<TEntity, TId>
        where TEntity : IEntity<TId>
        where TId : struct
    {
        public string Name { get; set; }
        public Expression<Func<TEntity, object>> Predicate { get; set; }
    }
}
