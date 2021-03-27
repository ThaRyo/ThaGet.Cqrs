using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Shared;
using ThaGet.Cqrs.Sort.Abstractions;
using ThaGet.Cqrs.Sort.Core.Enums;

namespace ThaGet.Cqrs.Sort
{
    public abstract class AbstractSort<TQuery, TEntity, TId> : IAbstractSort<TEntity, TId>
        where TQuery : class
        where TEntity : IEntity<TId>
        where TId : struct
    {
        public Dictionary<string, ISortDefinition<TEntity, TId>> Entries { get; }
        public Expression<Func<TEntity, object>> DefaultExpression { get; private set; }
        public SortDirection DefaultDirection { get; private set; }

        public AbstractSort()
        {
            Entries = new Dictionary<string, ISortDefinition<TEntity, TId>>();
        }

        public void CreateEntry(string query, Expression<Func<TEntity, object>> predicate)
        {
            ArgumentHelper.ThrowIfNullOrEmpty(query, nameof(query));
            ArgumentHelper.ThrowIfNull(predicate, nameof(predicate));

            if (Entries.ContainsKey(query))
                throw new ArgumentException("An entry with the same query name already exists.", nameof(query));

            Entries.Add(query, new SortDefinition<TEntity, TId>()
            {
                Name = query,
                Predicate = predicate
            });
        }

        public void SetDefault(Expression<Func<TEntity, object>> predicate, SortDirection direction)
        {
            ArgumentHelper.ThrowIfNull(predicate, nameof(predicate));

            // TODO What if called twice?
            DefaultExpression = predicate;
            DefaultDirection = direction;
        }
    }
}
