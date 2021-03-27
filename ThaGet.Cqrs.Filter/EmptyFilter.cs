using ThaGet.Cqrs.Domain.Entities.Abstractions;

namespace ThaGet.Cqrs.Filter
{
    public class EmptyFilter<TQuery, TEntity, TId> : AbstractFilter<TQuery, TEntity, TId>
        where TQuery : class
        where TEntity : IEntity<TId>
        where TId : struct
    {
    }
}
