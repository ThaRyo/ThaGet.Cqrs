namespace ThaGet.Cqrs.Domain.MySql.Entities
{
    public class TrackedEntityBase<TId> : VersionedEntityBase<TId>
        where TId : struct
    { }
}
