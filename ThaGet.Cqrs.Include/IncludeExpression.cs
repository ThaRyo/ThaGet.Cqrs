using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Include.Abstractions;

namespace ThaGet.Cqrs.Include
{
    public class IncludeExpression<TEntity, TId> : IIncludeExpression<TEntity, TId>, IEnumerable<Expression<Func<TEntity, object>>>
        where TEntity : class, IEntity<TId>
        where TId : struct
    {
        public ICollection<Expression<Func<TEntity, object>>> IncludeList { get; }

        public IncludeExpression()
        {
            IncludeList = new List<Expression<Func<TEntity, object>>>();
        }

        public void Add(Expression<Func<TEntity, object>> expression)
        {
            IncludeList.Add(expression);
        }

        public IEnumerator<Expression<Func<TEntity, object>>> GetEnumerator()
        {
            return IncludeList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
