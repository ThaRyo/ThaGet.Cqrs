using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Sort.Abstractions;
using ThaGet.Cqrs.Sort.Core.Enums;

namespace ThaGet.Cqrs.Sort
{
    public class SortExpression<TEntity, TId> : ISortExpression<TEntity, TId>
        where TEntity : IEntity<TId>
        where TId : struct
    {
        private readonly SortedDictionary<int, ISortRule<TEntity, TId>> _sortRules;
        private readonly Dictionary<string, Expression<Func<TEntity, object>>> _sortRuleCatalog;

        private ISortRule<TEntity, TId> _defaultRule;
        private int _counter = 1;

        public SortExpression()
        {
            _sortRules = new SortedDictionary<int, ISortRule<TEntity, TId>>();
            _sortRuleCatalog = new Dictionary<string, Expression<Func<TEntity, object>>>();
        }

        public void AddVariable(string name, Expression<Func<TEntity, object>> expression)
        {
            _sortRuleCatalog.Add(name, expression);
        }

        public void Add(Expression<Func<TEntity, object>> rule, SortDirection direction)
        {
            _sortRules.Add(_counter, new SortRule<TEntity, TId>(rule, direction));
            _counter++;
        }

        public void Add(ISortRule<TEntity, TId> rule)
        {
            _sortRules.Add(_counter, rule);
            _counter++;
        }

        public void Add(ISortInfo info)
        {
            var exists = _sortRuleCatalog.TryGetValue(info.Property, out var predicate);

            if (!exists)
            {
                // TODO Log warning? throw exception to log a warning?
            }
            else
            {
                Add(predicate, info.Direction);
            }
        }

        public void AddRange(IEnumerable<ISortRule<TEntity, TId>> infoList)
        {
            foreach (var info in infoList)
                Add(info);
        }

        public void AddRange(IEnumerable<ISortInfo> infoList)
        {
            foreach (var info in infoList)
                Add(info);
        }

        public void SetDefault(ISortRule<TEntity, TId> rule)
        {
            _defaultRule = rule;
        }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            bool firstRule = true;

            foreach (var rule in _sortRules.Values)
            {
                query = Sort(query, rule, firstRule);
                firstRule = false;
            }

            if (_defaultRule != null)
                query = Sort(query, _defaultRule, firstRule);

            return query;
        }

        private IQueryable<TEntity> Sort(IQueryable<TEntity> entities, ISortRule<TEntity, TId> rule, bool firstRule)
        {
            if (firstRule)
            {
                if (rule.Direction == SortDirection.Descending)
                    entities = entities.OrderByDescending(rule.Expression);
                else
                    entities = entities.OrderBy(rule.Expression);
            }
            else
            {
                if (rule.Direction == SortDirection.Descending)
                    entities = (entities as IOrderedQueryable<TEntity>).ThenByDescending(rule.Expression);
                else
                    entities = (entities as IOrderedQueryable<TEntity>).ThenBy(rule.Expression);
            }

            return entities;
        }
    }
}
