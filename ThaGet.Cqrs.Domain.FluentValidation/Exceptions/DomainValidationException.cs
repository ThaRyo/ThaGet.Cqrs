using System.Collections.Generic;
using ThaGet.Cqrs.Exceptions;
using Microsoft.AspNetCore.Http;

namespace ThaGet.Cqrs.Domain.FluentValidation.Exceptions
{
    public class DomainValidationException : ApiException
    {
        public static int ErrorCode { get; set; } = 4001;
        public IEnumerable<ValidationMessage> ValidationMessages { get; set; }

        public DomainValidationException(string message, IEnumerable<ValidationMessage> validationMessages)
            : base(ErrorCode, StatusCodes.Status400BadRequest, message)
        {
            ValidationMessages = validationMessages;
        }

        // TODO Domain validation errors should not be returned like contract validation errors
        // however, a middleware for logging those would be nice
    }
}
