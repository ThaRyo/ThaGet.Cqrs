using System;
using ThaGet.Cqrs.Exceptions;
using Microsoft.AspNetCore.Http;

namespace ThaGet.Cqrs.Domain.Exceptions
{
    public class ConcurrencyException : ApiException
    {
        public static int ErrorCode { get; set; } = 409;

        public ConcurrencyException(Exception ex)
            : base(ErrorCode, StatusCodes.Status409Conflict, ex)
        {
        }
    }
}
