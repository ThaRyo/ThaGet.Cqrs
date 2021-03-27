using System;
using System.Collections.Generic;
using System.Linq;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Filter.Abstractions;

namespace ThaGet.Cqrs.Filter
{
    public class FilterService<TId> : IFilterService<TId>
        where TId : struct
    {
        internal static List<Type> FilterTypeList { get; set; }

        public IReadOnlyCollection<IFilterDefinition<TEntity, TId>> GetFilterList<TQuery, TEntity>()
            where TQuery : class
            where TEntity : IEntity<TId>
        {
            var filter = GetFilter<TQuery, TEntity>();
            return filter.Entries.Values.ToList().AsReadOnly();
        }

        public void AddFilter<TQuery, TEntity>(IFilterExpression<TEntity, TId> expression)
            where TQuery : class
            where TEntity : IEntity<TId>
        {
            var filter = GetFilter<TQuery, TEntity>();

            foreach (var entry in filter.Entries.Values)
            {
                expression.AddVariable(entry.Name, entry.Predicate);
            }
        }

        private AbstractFilter<TQuery, TEntity, TId> GetFilter<TQuery, TEntity>()
            where TQuery : class
            where TEntity : IEntity<TId>
        {
            var validTypes = FilterTypeList
                .Where(type => type.IsSubclassOf(typeof(AbstractFilter<TQuery, TEntity, TId>)))
                .ToList();

            if (validTypes.Count == 1)
                return (AbstractFilter<TQuery, TEntity, TId>)Activator.CreateInstance(validTypes[0]);

            if (validTypes.Count > 1)
                throw new Exception($"Filter for type { typeof(TQuery).FullName } exists { validTypes.Count } times.");

            return new EmptyFilter<TQuery, TEntity, TId>();
        }
    }
}