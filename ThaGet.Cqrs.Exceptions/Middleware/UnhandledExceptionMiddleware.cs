using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ThaGet.Cqrs.Exceptions.Middleware
{
    public class UnhandledExceptionMiddleware : BaseExceptionMiddleware
    {
        public UnhandledExceptionMiddleware(RequestDelegate next, ILogger<BaseExceptionMiddleware> logger)
            : base(next, logger) { }

        public override async Task Invoke(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (Exception exception)
            {
                var apiException = new InternalServerException(exception);
                await HandleException(context, apiException);
            }
        }
    }
}
