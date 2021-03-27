using System;
using System.Linq.Expressions;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Filter.Abstractions;
using ThaGet.Cqrs.Filter.Core.Enums;

namespace ThaGet.Cqrs.Filter
{
    internal class FilterDefinition<TEntity, TId> : IFilterDefinition<TEntity, TId>
        where TEntity : IEntity<TId>
        where TId : struct
    {
        public string Name { get; set; }
        public FilterType Type { get; set; }
        public Expression<Func<TEntity, object>> Predicate { get; set; }
    }
}