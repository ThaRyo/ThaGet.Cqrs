using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TestClasses;
using ThaGet.Cqrs.Contract.Abstractions;
using ThaGet.Cqrs.Domain.Repositories.Abstractions;
using ThaGet.Cqrs.Filter.Abstractions;
using ThaGet.Cqrs.Handlers.Queries;
using ThaGet.Cqrs.Sort.Abstractions;

namespace HandlerSample
{
    public class TestEntityQuerySingleHandler : QueryEntitySingleHandler<TestEntityQueryRequest, TestEntityResponse, TestEntity, int>
    {
        public TestEntityQuerySingleHandler(ILogger<object> logger, IMapper mapper, IRepository<TestEntity, int> repository, IFilterService<int> filterService, ISortService<int> sortService)
            : base(logger, mapper, repository, filterService, sortService)
        {
        }

        protected override async Task<TestEntityResponse> Execute(TestEntityQueryRequest request, CancellationToken cancellationToken)
        {
            return new TestEntityResponse();
        }
    }
    public class TestEntityQueryRequest : IQuery<TestEntityResponse> { }
    public class TestEntityResponse { }
}
