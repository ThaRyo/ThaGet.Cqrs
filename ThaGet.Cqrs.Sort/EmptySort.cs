using ThaGet.Cqrs.Domain.Entities.Abstractions;

namespace ThaGet.Cqrs.Sort
{
    public class EmptySort<TQuery, TEntity, TId> : AbstractSort<TQuery, TEntity, TId>
        where TQuery : class
        where TEntity : IEntity<TId>
        where TId : struct
    {
    }
}
