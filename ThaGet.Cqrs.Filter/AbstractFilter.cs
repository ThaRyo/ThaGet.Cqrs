using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Filter.Abstractions;
using ThaGet.Cqrs.Filter.Core.Enums;
using ThaGet.Shared;

namespace ThaGet.Cqrs.Filter
{
    public abstract class AbstractFilter<TQuery, TEntity, TId> : IFilter<TEntity, TId>
        where TQuery : class
        where TEntity : IEntity<TId>
        where TId : struct
    {
        public Dictionary<string, IFilterDefinition<TEntity, TId>> Entries { get; set; }

        public AbstractFilter()
        {
            Entries = new Dictionary<string, IFilterDefinition<TEntity, TId>>();
        }

        public void CreateEntry(string query, FilterType type, Expression<Func<TEntity, object>> predicate)
        {
            ArgumentHelper.ThrowIfNullOrEmpty(query, nameof(query));
            ArgumentHelper.ThrowIfNull(predicate, nameof(predicate));

            if (Entries.ContainsKey(query))
                throw new ArgumentException("An entry with the same query name already exists.", nameof(query));

            Entries.Add(query, new FilterDefinition<TEntity, TId>()
            {
                Name = query,
                Type = type,
                Predicate = predicate
            });
        }
    }
}