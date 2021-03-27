using ThaGet.Cqrs.Contract.Abstractions;

namespace ThaGet.Cqrs.Contract.Commands
{
    public abstract class CreateCommand<TResponse> : ICommand<TResponse>
    {
    }
}
