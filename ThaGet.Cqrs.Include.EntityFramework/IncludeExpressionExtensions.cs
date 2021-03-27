using Microsoft.EntityFrameworkCore;
using System.Linq;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Include.Abstractions;
using ThaGet.Shared.Extensions;

namespace ThaGet.Cqrs.Include.EntityFramework
{
    public static class IncludeExpressionExtensions
    {
        public static IQueryable<TEntity> Apply<TEntity, TId>(this IIncludeExpression<TEntity, TId> expression, IQueryable<TEntity> query)
            where TEntity : class, IEntity<TId>
            where TId : struct
        {
            if (expression.IncludeList == null)
                return query;

            return expression.IncludeList.Aggregate(
                query,
                (current, include) => current.Include(include.AsPath())
            );
        }
    }
}
