using AutoMapper;
using ThaGet.Cqrs.Contract.Abstractions;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Domain.Repositories.Abstractions;
using Microsoft.Extensions.Logging;

namespace ThaGet.Cqrs.Handlers.Commands
{
    public abstract class CommandEntityHandler<TCommand, TResponse, TEntity, TId> : EntityHandler<TCommand, TResponse, TEntity, TId>
        where TCommand : ICommand<TResponse>
        where TEntity : IEntity<TId>
        where TId : struct
    {
        protected CommandEntityHandler(ILogger<object> logger, IMapper mapper, IRepository<TEntity, TId> repository) : base(logger, mapper, repository)
        {
        }
    }
}
