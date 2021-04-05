using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ThaGet.Cqrs.Contract.Commands;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Domain.Repositories.Abstractions;
using Microsoft.Extensions.Logging;

namespace ThaGet.Cqrs.Handlers.Commands
{
    public abstract class CreateEntityHandler<TCommand, TEntity, TId>
        : CommandEntityHandler<TCommand, TId, TEntity, TId>
        where TCommand : CreateEntityCommand<TId>
        where TEntity : IEntity<TId>
        where TId : struct
    {
        protected TEntity MappedCommand { get; private set; }

        protected CreateEntityHandler(ILogger<object> logger, IMapper mapper, IRepository<TEntity, TId> repository) : base(logger, mapper, repository)
        {
        }

        protected override void Configure(TCommand command)
        {
            MappedCommand = Mapper.Map<TEntity>(command);
        }

        protected override async Task<TId> Execute(TCommand command, CancellationToken cancellationToken)
        {
            Repository.Create(MappedCommand);
            await Repository.UnitOfWork.SaveAllChangesAsync();

            return MappedCommand.Id;
        }
    }
}
