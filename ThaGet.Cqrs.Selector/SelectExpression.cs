using System;
using System.Linq;
using System.Linq.Expressions;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Selector.Abstractions;

namespace ThaGet.Cqrs.Selector
{
    public class SelectExpression<TSource, TResult, TId> : ISelectExpression<TSource, TResult, TId>
        where TSource : IEntity<TId>
        where TId : struct
    {
        private readonly Expression<Func<TSource, TResult>> _mapping;
        private readonly Func<IQueryable<TSource>, IQueryable<TResult>> _projection;

        protected SelectExpression(Expression<Func<TSource, TResult>> mapping)
        {
            _mapping = mapping;
        }

        protected SelectExpression(Func<IQueryable<TSource>, IQueryable<TResult>> projection)
        {
            _projection = projection;
        }

        public IQueryable<TResult> Apply(IQueryable<TSource> query)
        {
            if (_projection != null)
                return _projection(query);

            return query.Select(_mapping);
        }

        public static SelectExpression<TSource, TResult, TId> WithMap(Expression<Func<TSource, TResult>> selector)
        {
            return new SelectExpression<TSource, TResult, TId>(mapping: selector);
        }

        public static SelectExpression<TSource, TResult, TId> WithParameters(Func<IQueryable<TSource>, IQueryable<TResult>> selector)
        {
            return new SelectExpression<TSource, TResult, TId>(projection: selector);
        }
    }
}
