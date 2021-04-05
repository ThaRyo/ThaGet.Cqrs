using ThaGet.Cqrs.Contract.Abstractions;

namespace ThaGet.Cqrs.Contract.Commands
{
    public abstract class UpdateEntityCommand<TId, TResponse> : ICommand<TResponse>
        where TId : struct
    {
        public TId Id { get; set; }
    }
}
