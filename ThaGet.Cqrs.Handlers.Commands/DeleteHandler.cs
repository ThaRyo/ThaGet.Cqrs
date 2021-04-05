using ThaGet.Cqrs.Contract.Commands;
using Microsoft.Extensions.Logging;
using ThaGet.Cqrs.Handlers.Core;

namespace ThaGet.Cqrs.Handlers.Commands
{
    public abstract class DeleteHandler<TCommand, TResponse>
        : BaseHandler<TCommand, TResponse>
        where TCommand : DeleteCommand<TResponse>
    {
        protected DeleteHandler(ILogger<object> logger) : base(logger)
        {
        }
    }
}
