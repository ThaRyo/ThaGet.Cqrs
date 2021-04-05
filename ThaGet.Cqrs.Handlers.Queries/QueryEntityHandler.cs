using AutoMapper;
using ThaGet.Cqrs.Contract.Abstractions;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Domain.Repositories.Abstractions;
using Microsoft.Extensions.Logging;
using ThaGet.Cqrs.Sort;
using ThaGet.Cqrs.Filter;
using ThaGet.Cqrs.Include.Abstractions;
using ThaGet.Cqrs.Filter.Abstractions;
using ThaGet.Cqrs.Sort.Abstractions;
using ThaGet.Cqrs.Include;
using ThaGet.Cqrs.Handlers.Core;

namespace ThaGet.Cqrs.Handlers.Queries
{
    public abstract class QueryEntityHandler<TQuery, TResponse, TEntity, TId>
        : EntityBaseHandler<TQuery, TResponse, TEntity, TId>
        where TQuery : class, IQuery<TResponse>
        where TResponse : class
        where TEntity : class, IEntity<TId>
        where TId : struct
    {
        private readonly IFilterService<TId> _filterService;
        private readonly ISortService<TId> _sortService;

        /// <summary>
        /// Parameters for AutoMapper.
        /// Assigning an anonymous object to it will use the ProjectTo method, else Map will be used.
        /// </summary>
        protected dynamic Parameters { get; set; }

        protected IIncludeExpression<TEntity, TId> Include { get; private set; }
        protected IFilterExpression<TEntity, TId> Filter { get; private set; }
        protected ISortExpression<TEntity, TId> Sort { get; private set; }

        protected QueryEntityHandler(ILogger<object> logger, IMapper mapper, IRepository<TEntity, TId> repository, IFilterService<TId> filterService, ISortService<TId> sortService)
            : base(logger, mapper, repository)
        {
            _filterService = filterService;
            _sortService = sortService;
        }

        protected override void Configure(TQuery request)
        {
            Include = GetIncludes();
            Filter = GetFilterExpression(request);
            Sort = GetSortExpression(request);
        }

        protected virtual IIncludeExpression<TEntity, TId> GetIncludes() => new IncludeExpression<TEntity, TId>();

        protected virtual IFilterExpression<TEntity, TId> GetFilterExpression(TQuery request)
        {
            var filterExpression = new FilterExpression<TEntity, TId>();

            // Add rules and possible filters discovered by reflection for type TQuery
            _filterService.AddFilter<TQuery, TEntity>(filterExpression);

            return filterExpression;
        }

        protected virtual ISortExpression<TEntity, TId> GetSortExpression(TQuery request)
        {
            var sortExpression = new SortExpression<TEntity, TId>();

            // Add rules and possible sorts discovered by reflection for type TQuery
            _sortService.AddSort<TQuery, TEntity>(sortExpression);

            return sortExpression;
        }
    }
}