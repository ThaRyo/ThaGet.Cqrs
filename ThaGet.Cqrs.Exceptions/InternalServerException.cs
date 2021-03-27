using System;
using Microsoft.AspNetCore.Http;

namespace ThaGet.Cqrs.Exceptions
{
    public class InternalServerException : ApiException
    {
        public static int ErrorCode { get; set; } = 500;

        public InternalServerException(Exception ex) : base(ErrorCode, StatusCodes.Status500InternalServerError, ex)
        {
        }
    }
}
