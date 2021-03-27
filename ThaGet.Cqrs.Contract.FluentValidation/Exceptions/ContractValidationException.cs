using System.Collections.Generic;
using ThaGet.Cqrs.Exceptions;
using Microsoft.AspNetCore.Http;

namespace ThaGet.Cqrs.Contract.FluentValidation.Exceptions
{
    public class ContractValidationException : ApiException
    {
        public static int ErrorCode { get; set; } = 4001;
        public IEnumerable<ValidationMessage> ValidationMessages { get; }

        public ContractValidationException(params ValidationMessage[] validationMessages)
            : base(ErrorCode, StatusCodes.Status400BadRequest)
        {
            ValidationMessages = validationMessages;

            // NotNullValidator | ValuatorId

            // Fluent_Code_NotNullValidator -> "{0} darf nicht leer sein"
            // Fluent_Param_ValuatorId -> "Bewerter"
        }

        public override ApiError ToError() => new ContractValidationError(ErrorCode, ValidationMessages);
    }
}
