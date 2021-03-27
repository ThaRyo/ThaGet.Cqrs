using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ThaGet.Cqrs.Exceptions.Middleware
{
    public class ApiExceptionMiddleware : BaseExceptionMiddleware
    {
        public ApiExceptionMiddleware(RequestDelegate next, ILogger<BaseExceptionMiddleware> logger)
            : base(next, logger) { }

        public override async Task Invoke(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (ApiException apiException)
            {
                await HandleException(context, apiException);
            }
        }
    }
}
