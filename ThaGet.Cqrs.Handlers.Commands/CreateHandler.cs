using ThaGet.Cqrs.Contract.Commands;
using Microsoft.Extensions.Logging;
using ThaGet.Cqrs.Handlers.Core;

namespace ThaGet.Cqrs.Handlers.Commands
{
    public abstract class CreateHandler<TCommand, TResponse>
        : BaseHandler<TCommand, TResponse>
        where TCommand : CreateCommand<TResponse>
    {
        protected CreateHandler(ILogger<object> logger) : base(logger)
        {
        }
    }
}
