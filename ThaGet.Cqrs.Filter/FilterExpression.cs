using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Filter.Abstractions;
using ThaGet.Cqrs.Filter.Internal;

namespace ThaGet.Cqrs.Filter
{
    public class FilterExpression<TEntity, TId> : IFilterExpression<TEntity, TId>, IEnumerable<Expression<Func<TEntity, bool>>>
        where TEntity : IEntity<TId>
        where TId : struct
    {
        private readonly SortedDictionary<int, Expression<Func<TEntity, bool>>> _filterRules;
        private readonly FilterPredicateBuilder<TEntity, TId> _filterBuilder;

        private int _counter = 1;

        public FilterExpression()
        {
            _filterRules = new SortedDictionary<int, Expression<Func<TEntity, bool>>>();
            _filterBuilder = new FilterPredicateBuilder<TEntity, TId>();
        }

        public void AddVariable(string name, Expression<Func<TEntity, object>> expression)
        {
            _filterBuilder.AddVariableDefinition(name, expression);
        }

        public void Add(Expression<Func<TEntity, bool>> rule)
        {
            _filterRules.Add(_counter, rule);
            _counter++;
        }

        public void Add(IFilterInfo info)
        {
            var predicate = _filterBuilder.BuildFromString(info.Property, info.Operator, info.Value);

            if (predicate == null)
            {
                // Log warning? throw exception to log a warning?
            }
            else
            {
                Add(predicate);
            }
        }

        public void AddRange(IEnumerable<Expression<Func<TEntity, bool>>> ruleList)
        {
            foreach (var rule in ruleList)
                Add(rule);
        }

        public void AddRange(IEnumerable<IFilterInfo> infoList)
        {
            // TODO if Add(FilterInfo info) throws exception -> ?
            foreach (var info in infoList)
                Add(info);
        }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            foreach (var rule in _filterRules)
                query = query.Where(rule.Value);

            return query;
        }

        public IEnumerator<Expression<Func<TEntity, bool>>> GetEnumerator()
        {
            return _filterRules.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}