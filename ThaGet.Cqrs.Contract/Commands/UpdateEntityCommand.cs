using ThaGet.Cqrs.Contract.Abstractions;

namespace ThaGet.Cqrs.Contract.Commands
{
    public abstract class UpdateEntityCommand<TResponse, TId> : ICommand<TResponse>
        where TId : struct
    {
        public TId Id { get; set; }
        public byte[] Timestamp { get; set; }
    }
}
