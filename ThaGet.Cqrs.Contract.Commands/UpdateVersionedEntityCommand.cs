namespace ThaGet.Cqrs.Contract.Commands
{
    public abstract class UpdateVersionedEntityCommand<TId, TRowVersion, TResponse> : UpdateEntityCommand<TId, TResponse>
        where TId : struct
    {
        public TRowVersion RowVersion { get; set; }
    }
}
