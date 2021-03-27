using ThaGet.Cqrs.Contract.Commands;
using Microsoft.Extensions.Logging;

namespace ThaGet.Cqrs.Handlers.Commands
{
    public abstract class UpdateHandler<TCommand, TResponse> : BaseHandler<TCommand, TResponse>
        where TCommand : UpdateCommand<TResponse>
    {
        protected UpdateHandler(ILogger<object> logger) : base(logger)
        {
        }
    }
}
