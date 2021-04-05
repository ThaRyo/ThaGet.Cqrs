using ThaGet.Cqrs.Domain.Entities.Abstractions;

namespace ThaGet.Cqrs.Domain.Entities
{
    public abstract class VersionedEntityBase<TId, TRowVersion> : EntityBase<TId>, IVersionedEntity<TRowVersion>
        where TId : struct
    {
        public TRowVersion RowVersion { get; set; }
    }
}
