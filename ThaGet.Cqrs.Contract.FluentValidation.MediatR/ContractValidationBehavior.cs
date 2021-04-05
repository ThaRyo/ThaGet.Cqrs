using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace ThaGet.Cqrs.Contract.FluentValidation
{
    public class ContractValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IValidator<TRequest> _validator;

        public ContractValidationBehavior(IValidator<TRequest> validator = null)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validator == null)
            {
                var response = await next();
                return response;
            }

            var validationResult = _validator.Validate(request);
            if (validationResult.IsValid)
            {
                var response = await next();
                return response;
            }

            throw new Exceptions.ContractValidationException(
                validationResult.Errors
                .Select(x => new ValidationMessage(x.ErrorCode, x.PropertyName))
                .ToArray()
            );
        }
    }
}
