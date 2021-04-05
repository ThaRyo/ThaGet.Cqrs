namespace ThaGet.Cqrs.Domain.MySql.Entities
{
    public class VersionedEntityBase<TId> : Domain.Entities.VersionedEntityBase<TId, byte[]>
        where TId : struct
    {
    }
}
