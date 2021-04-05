using ThaGet.Cqrs.Domain.Entities.Abstractions;

namespace ThaGet.Cqrs.Domain.Entities
{
    public abstract class EntityBase<TId> : IEntity<TId>
        where TId : struct
    {
        protected EntityBase()
        {
        }

        public TId Id { get; set; }
    }
}
