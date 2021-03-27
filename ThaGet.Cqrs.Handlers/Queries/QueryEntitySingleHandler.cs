using System.Threading.Tasks;
using AutoMapper;
using ThaGet.Cqrs.Contract.Abstractions;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Domain.Repositories.Abstractions;
using ThaGet.Cqrs.Filter.Abstractions;
using Microsoft.Extensions.Logging;
using ThaGet.Cqrs.Selector;
using ThaGet.Cqrs.Selector.Abstractions;
using ThaGet.Cqrs.Sort.Abstractions;

namespace ThaGet.Cqrs.Handlers.Queries
{
    public abstract class QueryEntitySingleHandler<TQuery, TResponse, TEntity, TId> : QueryEntityHandler<TQuery, TResponse, TEntity, TId>
       where TQuery : class, IQuery<TResponse>
       where TResponse : class
       where TEntity : class, IEntity<TId>
        where TId : struct
    {
        protected ISelectExpression<TEntity, TResponse, TId> Select { get; private set; }

        protected QueryEntitySingleHandler(ILogger<object> logger, IMapper mapper, IRepository<TEntity, TId> repository, IFilterService<TId> filterService, ISortService<TId> sortService)
            : base(logger, mapper, repository, filterService, sortService)
        {
        }

        protected override void Configure(TQuery request)
        {
            base.Configure(request);
            Select = GetSelectExpression();

            // TODO If TQuery == IQueryEntitySingle -> set property Id
            // TODO Set property MustExist in sub-class (get/find)
            // TODO Then we could define Execute here and choose action based on whether
            // Id != null and MustExist == true/false
        }

        // TODO Enum for action (GetById, GetBy, FindById, FindBy) to be set in constructor/Configure which decides which of the following methods will be called in Execute automatically
        public async Task<TResponse> GetByIdAsync(TId id) => await Repository.GetByIdAsync(id, Select, Include);

        public async Task<TResponse> GetByAsync() => await Repository.GetByAsync(Select, Include, Filter);

        public async Task<TResponse> FindByIdAsync(TId id) => await Repository.FindByIdAsync(id, Select, Include);

        public async Task<TResponse> FindByAsync() => await Repository.FindByAsync(Select, Include, Filter);

        protected virtual ISelectExpression<TEntity, TResponse, TId> GetSelectExpression()
        {
            if (Parameters != null)
                return SelectExpression<TEntity, TResponse, TId>.WithParameters(
                    o => Mapper.ProjectTo<TResponse>(o, Parameters)
                );

            return SelectExpression<TEntity, TResponse, TId>.WithMap(
                o => Mapper.Map<TResponse>(o)
            );
        }
    }
}