using System;

namespace ThaGet.Cqrs.Domain.MySql.Entities
{
    public class VersionedEntityBase<TId> : Domain.Entities.VersionedEntityBase<TId, DateTime>
        where TId : struct
    {
    }
}
