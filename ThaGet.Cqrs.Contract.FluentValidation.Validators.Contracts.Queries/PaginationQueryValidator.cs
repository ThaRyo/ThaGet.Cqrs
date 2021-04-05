using FluentValidation;
using ThaGet.Cqrs.Contract.Queries;

namespace ThaGet.Cqrs.Contract.FluentValidation.Validators
{
    public class PaginationQueryValidator<TQuery, TResponse> : AbstractValidator<TQuery>
        where TQuery : EntityPaginationQuery<TResponse>
        where TResponse : class
    {
        public PaginationQueryValidator()
        {
            // TODO Regular expression for filter / sort ?
            // TODO Start page at 0 or 1 ?
            RuleFor(x => x.Page).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
        }
    }
}
