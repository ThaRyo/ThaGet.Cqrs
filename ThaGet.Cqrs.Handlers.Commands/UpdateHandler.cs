using ThaGet.Cqrs.Contract.Commands;
using Microsoft.Extensions.Logging;
using ThaGet.Cqrs.Handlers.Core;

namespace ThaGet.Cqrs.Handlers.Commands
{
    public abstract class UpdateHandler<TCommand, TResponse> 
        : BaseHandler<TCommand, TResponse>
        where TCommand : UpdateCommand<TResponse>
    {
        protected UpdateHandler(ILogger<object> logger) : base(logger)
        {
        }
    }
}
