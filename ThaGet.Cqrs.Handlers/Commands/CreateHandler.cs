using ThaGet.Cqrs.Contract.Commands;
using Microsoft.Extensions.Logging;

namespace ThaGet.Cqrs.Handlers.Commands
{
    public abstract class CreateHandler<TCommand, TResponse> : BaseHandler<TCommand, TResponse>
        where TCommand : CreateCommand<TResponse>
    {
        protected CreateHandler(ILogger<object> logger) : base(logger)
        {
        }
    }
}
