using System;
using ThaGet.Cqrs.Domain.Entities.Abstractions;

namespace ThaGet.Cqrs.Domain.Entities
{
    public abstract class TrackedEntityBase<TId, TRowVersion> : VersionedEntityBase<TId, TRowVersion>, ITrackedEntity
        where TId : struct
    {
        public TrackedEntityBase()
        {
            CreatedAtUtc = DateTime.UtcNow;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public DateTime CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
    }
}
