using System.Collections.Generic;
using ThaGet.Cqrs.Exceptions;

namespace ThaGet.Cqrs.Contract.FluentValidation.Exceptions
{
    public class ContractValidationError : ApiError
    {
        public IEnumerable<ValidationMessage> ValidationMessages { get; }

        public ContractValidationError(int errorCode, IEnumerable<ValidationMessage> messages) : base(errorCode)
        {
            ValidationMessages = messages;
        }
    }
}
