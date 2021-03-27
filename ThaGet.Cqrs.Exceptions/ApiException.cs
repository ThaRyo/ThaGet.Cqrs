using System;

namespace ThaGet.Cqrs.Exceptions
{
    public abstract class ApiException : Exception
    {
        public int Code { get; }
        public int StatusCode { get; }

        public ApiException(int errorCode, int statusCode) : this(errorCode, statusCode, null, null)
        { }

        public ApiException(int errorCode, int statusCode, string message) : this(errorCode, statusCode, message, null)
        { }

        public ApiException(int errorCode, int statusCode, Exception innerException) : this(errorCode, statusCode, null, innerException)
        { }

        private ApiException(int errorCode, int statusCode, string message, Exception innerException) : base(message, innerException)
        {
            Code = errorCode;
            StatusCode = statusCode;
        }

        public virtual ApiError ToError() => new ApiError(Code);
    }
}
