using ThaGet.Cqrs.Contract.Commands;
using Microsoft.Extensions.Logging;

namespace ThaGet.Cqrs.Handlers.Commands
{
    public abstract class CommandHandler<TCommand, TResponse> : BaseHandler<TCommand, TResponse>
        where TCommand : Command<TResponse>
    {
        protected CommandHandler(ILogger<object> logger) : base(logger)
        {
        }
    }
}
