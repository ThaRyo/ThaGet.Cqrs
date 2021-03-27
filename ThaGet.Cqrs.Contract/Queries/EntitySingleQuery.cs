using ThaGet.Cqrs.Contract.Abstractions;

namespace ThaGet.Cqrs.Contract.Queries
{
    public abstract class EntitySingleQuery<TResponse> : IQuery<TResponse>
    {
    }
}
