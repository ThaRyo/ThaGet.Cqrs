using System;
using System.Collections.Generic;
using System.Linq;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Sort.Abstractions;

namespace ThaGet.Cqrs.Sort
{
    public class SortService<TId> : ISortService<TId>
        where TId : struct
    {
        internal static List<Type> SortTypeList { get; set; }

        public IReadOnlyCollection<ISortDefinition<TEntity, TId>> GetSortList<TQuery, TEntity>()
            where TQuery : class
            where TEntity : IEntity<TId>
        {
            var sort = GetSort<TQuery, TEntity>();

            return sort.Entries.Values.ToList()
                .AsReadOnly();
        }

        public void AddSort<TQuery, TEntity>(ISortExpression<TEntity, TId> expression)
            where TQuery : class
            where TEntity : IEntity<TId>
        {
            var sort = GetSort<TQuery, TEntity>();

            foreach (var entry in sort.Entries.Values)
                expression.AddVariable(entry.Name, entry.Predicate);

            if (sort.DefaultExpression != null)
                expression.SetDefault(new SortRule<TEntity, TId>(sort.DefaultExpression, sort.DefaultDirection));
        }

        private AbstractSort<TQuery, TEntity, TId> GetSort<TQuery, TEntity>()
            where TQuery : class
            where TEntity : IEntity<TId>
        {
            var validTypes = SortTypeList
                .Where(type => type.IsSubclassOf(typeof(AbstractSort<TQuery, TEntity, TId>)))
                .ToList();

            if (validTypes.Count == 1)
                return (AbstractSort<TQuery, TEntity, TId>)Activator.CreateInstance(validTypes[0]);

            if (validTypes.Count > 1)
                throw new Exception($"Sort for type { typeof(TQuery).FullName } exists { validTypes.Count } times.");

            return new EmptySort<TQuery, TEntity, TId>();
        }
    }
}
