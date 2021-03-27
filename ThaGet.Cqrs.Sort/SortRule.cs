using System;
using System.Linq.Expressions;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Sort.Abstractions;
using ThaGet.Cqrs.Sort.Core.Enums;

namespace ThaGet.Cqrs.Sort
{
    public class SortRule<TEntity, TId> : ISortRule<TEntity, TId>
        where TEntity : IEntity<TId>
        where TId : struct
    {
        public Expression<Func<TEntity, object>> Expression { get; }
        public SortDirection Direction { get; }

        public SortRule(Expression<Func<TEntity, object>> expression, SortDirection direction)
        {
            Expression = expression;
            Direction = direction;
        }
    }
}
