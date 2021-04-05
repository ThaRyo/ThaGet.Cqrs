using System.Threading.Tasks;
using AutoMapper;
using ThaGet.Cqrs.Contract.Abstractions;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Domain.Repositories.Abstractions;
using Microsoft.Extensions.Logging;
using ThaGet.Cqrs.Filter.Abstractions;
using ThaGet.Cqrs.Selector.Abstractions;
using ThaGet.Cqrs.Selector;
using System.Collections.Generic;
using ThaGet.Cqrs.Sort.Abstractions;

namespace ThaGet.Cqrs.Handlers.Queries
{
    public abstract class QueryEntityCollectionHandler<TQuery, TResponse, TResponseType, TEntity, TId>
        : QueryEntityHandler<TQuery, TResponse, TEntity, TId>
        where TQuery : class, IQuery<TResponse>
        where TResponse : class, IEnumerable<TResponseType>
        where TResponseType : class
        where TEntity : class, IEntity<TId>
        where TId : struct
    {
        protected ISelectExpression<TEntity, TResponseType, TId> Select { get; private set; }

        protected QueryEntityCollectionHandler(ILogger<object> logger, IMapper mapper, IRepository<TEntity, TId> repository, IFilterService<TId> filterService, ISortService<TId> sortService)
            : base(logger, mapper, repository, filterService, sortService)
        {
        }

        protected override void Configure(TQuery request)
        {
            base.Configure(request);
            Select = GetSelectExpression();
        }

        public async Task<TResponse> FindAllByAsync() => (TResponse)await Repository.FindAllByAsync(Select, Include, Filter, Sort);

        protected ISelectExpression<TEntity, TResponseType, TId> GetSelectExpression()
        {
            if (Parameters != null)
                return SelectExpression<TEntity, TResponseType, TId>.WithParameters(
                    o => Mapper.ProjectTo<TResponseType>(o, Parameters)
                );

            return SelectExpression<TEntity, TResponseType, TId>.WithMap(
                o => Mapper.Map<TResponseType>(o)
            );
        }
    }
}