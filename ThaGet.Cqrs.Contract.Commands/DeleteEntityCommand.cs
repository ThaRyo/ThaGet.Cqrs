using ThaGet.Cqrs.Contract.Abstractions;

namespace ThaGet.Cqrs.Contract.Commands
{
    public abstract class DeleteEntityCommand<TId, TResponse> : ICommand<TResponse>
    {
        public TId Id { get; set; }
    }
}
