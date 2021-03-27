using ThaGet.Cqrs.Contract.Abstractions;

namespace ThaGet.Cqrs.Contract.Commands
{
    public abstract class DeleteEntityCommand<TResponse> : ICommand<TResponse>
    {
    }
}
