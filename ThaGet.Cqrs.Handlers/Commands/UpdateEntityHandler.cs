using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ThaGet.Cqrs.Contract.Commands;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Domain.Repositories.Abstractions;
using Microsoft.Extensions.Logging;

namespace ThaGet.Cqrs.Handlers.Commands
{
    public abstract class UpdateEntityHandler<TCommand, TEntity, TId> : CommandEntityHandler<TCommand, byte[], TEntity, TId>
        where TCommand : UpdateEntityCommand<byte[], TId>
        where TEntity : IEntity<TId>
        where TId : struct
    {
        protected TEntity MappedCommand { get; private set; }

        protected UpdateEntityHandler(ILogger<object> logger, IMapper mapper, IRepository<TEntity, TId> repository) : base(logger, mapper, repository)
        {
        }

        protected override void Configure(TCommand command)
        {
            MappedCommand = Mapper.Map<TEntity>(command);
        }

        protected override async Task<byte[]> Execute(TCommand request, CancellationToken cancellationToken)
        {
            Repository.Update(MappedCommand);
            await Repository.UnitOfWork.SaveAllChangesAsync();

            return MappedCommand.Timestamp;
        }
    }
}
