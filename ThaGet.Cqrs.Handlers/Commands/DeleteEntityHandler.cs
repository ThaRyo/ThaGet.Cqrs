using AutoMapper;
using ThaGet.Cqrs.Contract.Commands;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Domain.Repositories.Abstractions;
using Microsoft.Extensions.Logging;

namespace ThaGet.Cqrs.Handlers.Commands
{
    public abstract class DeleteEntityHandler<TCommand, TResponse, TEntity, TId> : CommandEntityHandler<TCommand, TResponse, TEntity, TId>
        where TCommand : DeleteEntityCommand<TResponse>
        where TEntity : IEntity<TId>
        where TId : struct
    {
        protected DeleteEntityHandler(ILogger<object> logger, IMapper mapper, IRepository<TEntity, TId> repository) : base(logger, mapper, repository)
        {
        }
    }
}
