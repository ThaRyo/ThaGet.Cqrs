using ThaGet.Cqrs.Contract.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ThaGet.Cqrs.Contract.Queries
{
    public abstract class EntityPaginationQuery<TResponse> : IQuery<TResponse>
    {
        [FromQuery]
        public string Filter { get; set; }
        [FromQuery]
        public string Sort { get; set; }
        [FromQuery]
        public int Page { get; set; }
        [FromQuery]
        public int PageSize { get; set; }
    }
}
