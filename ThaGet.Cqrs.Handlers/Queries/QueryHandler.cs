using ThaGet.Cqrs.Contract.Abstractions;
using Microsoft.Extensions.Logging;

namespace ThaGet.Cqrs.Handlers.Queries
{
    public abstract class QueryHandler<TQuery, TResponse> : BaseHandler<TQuery, TResponse>
        where TQuery : class, IQuery<TResponse>
    {
        protected QueryHandler(ILogger<object> logger) : base(logger)
        {
        }
    }
}
