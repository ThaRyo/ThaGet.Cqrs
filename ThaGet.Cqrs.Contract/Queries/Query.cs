using ThaGet.Cqrs.Contract.Abstractions;

namespace ThaGet.Cqrs.Contract.Queries
{
    public abstract class Query<TResponse> : IQuery<TResponse>
    {
    }
}