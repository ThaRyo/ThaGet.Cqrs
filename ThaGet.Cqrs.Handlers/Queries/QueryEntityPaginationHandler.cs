using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ThaGet.Cqrs.Contract.Queries;
using ThaGet.Cqrs.Domain.Abstractions;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Domain.Repositories.Abstractions;
using ThaGet.Cqrs.Filter;
using ThaGet.Cqrs.Filter.Abstractions;
using Microsoft.Extensions.Logging;
using ThaGet.Cqrs.Selector;
using ThaGet.Cqrs.Selector.Abstractions;
using ThaGet.Cqrs.Sort;
using ThaGet.Cqrs.Sort.Abstractions;

namespace ThaGet.Cqrs.Handlers.Queries
{
    public abstract class QueryEntityPaginationHandler<TQuery, TResponse, TResponseType, TEntity, TId> : QueryEntityHandler<TQuery, TResponse, TEntity, TId>
        where TQuery : EntityPaginationQuery<TResponse>
        where TResponse : class, IPagination<TResponseType>
        where TResponseType : class
        where TEntity : class, IEntity<TId>
        where TId : struct
    {
        protected ISelectExpression<TEntity, TResponseType, TId> Select { get; private set; }
        protected int Skip { get; private set; }
        protected int Take { get; private set; }

        protected QueryEntityPaginationHandler(ILogger<object> logger, IMapper mapper, IRepository<TEntity, TId> repository, IFilterService<TId> filterService, ISortService<TId> sortService)
            : base(logger, mapper, repository, filterService, sortService)
        {
        }

        protected override void Configure(TQuery request)
        {
            base.Configure(request);

            Select = GetSelectExpression();
            Skip = request.Page * request.PageSize;
            Take = request.PageSize;
        }

        // TODO Sealed may be too restricting for some edge cases?
        protected override sealed async Task<TResponse> Execute(TQuery request, CancellationToken cancellationToken)
            => (TResponse)await Repository.GetPaginationAsync(Select, Include, Filter, Sort, Skip, Take);

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

        protected override IFilterExpression<TEntity, TId> GetFilterExpression(TQuery request)
        {
            var filterExpression = base.GetFilterExpression(request);

            // Add rules using previously discovered filters if such parameters exists
            if (!string.IsNullOrEmpty(request.Filter))
            {
                var filterParams = request.Filter.Trim().Split(',');
                var filterInfos = filterParams
                    .Select(o => FilterParser.Parse(o))
                    // Ignore invalid options
                    .Where(o => o != null)
                    .ToArray();

                filterExpression.AddRange(filterInfos);
            }

            return filterExpression;
        }

        protected override ISortExpression<TEntity, TId> GetSortExpression(TQuery request)
        {
            var sortExpression = base.GetSortExpression(request);

            // Add rules using previously discovered sorts if such parameters exists
            if (!string.IsNullOrEmpty(request.Sort))
            {
                var sortParams = request.Sort.Trim().Split(',');
                var sortInfos = sortParams
                    .Select(o => SortParser.Parse(o))
                    // Ignore invalid options
                    .Where(o => o != null)
                    .ToArray();

                sortExpression.AddRange(sortInfos);
            }

            return sortExpression;
        }
    }
}
