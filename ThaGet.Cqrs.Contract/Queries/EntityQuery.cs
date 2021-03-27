using ThaGet.Cqrs.Contract.Abstractions;

namespace ThaGet.Cqrs.Contract.Queries
{
    public abstract class EntityQuery<TResponse> : IQuery<TResponse>
    {
    }
}
