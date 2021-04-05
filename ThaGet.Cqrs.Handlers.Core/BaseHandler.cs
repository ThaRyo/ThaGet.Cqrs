using System.Threading;
using System.Threading.Tasks;
using ThaGet.Cqrs.Contract.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ThaGet.Cqrs.Handlers.Core
{
    public abstract class BaseHandler<TContract, TResponse> : IRequestHandler<TContract, TResponse>
        where TContract : IContract<TResponse>
    {
        protected ILogger<object> Logger { get; }

        protected BaseHandler(ILogger<object> logger)
        {
            Logger = logger;
        }

        public async Task<TResponse> Handle(TContract request, CancellationToken cancellationToken)
        {
            // Permission check by sub-class
            await CheckPermissions(request, cancellationToken);

            // TODO Rename to "Prepare" ?
            // Configure stuff
            Configure(request);

            // Execution controlled by sub-class
            return await Execute(request, cancellationToken);
        }

        protected virtual Task CheckPermissions(TContract request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual void Configure(TContract contract)
        {
        }

        protected abstract Task<TResponse> Execute(TContract request, CancellationToken cancellationToken);
    }
}
