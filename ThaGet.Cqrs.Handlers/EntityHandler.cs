using AutoMapper;
using ThaGet.Cqrs.Contract.Abstractions;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Domain.Repositories.Abstractions;
using Microsoft.Extensions.Logging;

namespace ThaGet.Cqrs.Handlers
{
    public abstract class EntityHandler<TCommand, TResponse, TEntity, TId> : BaseHandler<TCommand, TResponse>
        where TCommand : IContract<TResponse>
        where TEntity : IEntity<TId>
        where TId : struct
    {
        protected IMapper Mapper { get; }
        protected IRepository<TEntity, TId> Repository { get; }

        protected EntityHandler(ILogger<object> logger, IMapper mapper, IRepository<TEntity, TId> repository) : base(logger)
        {
            Mapper = mapper;
            Repository = repository;
        }
    }
}
