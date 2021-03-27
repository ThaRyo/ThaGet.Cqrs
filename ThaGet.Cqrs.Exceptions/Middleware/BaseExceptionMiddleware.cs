using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ThaGet.Cqrs.Exceptions.Middleware
{
    public abstract class BaseExceptionMiddleware
    {
        protected RequestDelegate Next { get; }

        protected ILogger<BaseExceptionMiddleware> Logger { get; }

        public BaseExceptionMiddleware(RequestDelegate next, ILogger<BaseExceptionMiddleware> logger)
        {
            Next = next;
            Logger = logger;
        }

        public abstract Task Invoke(HttpContext context);

        protected async Task HandleException(HttpContext context, ApiException apiException)
        {
            Logger.LogError(apiException, "Exception occured");
            await SendErrorAsync(context, apiException);
        }

        protected async Task SendErrorAsync(HttpContext context, ApiException apiException)
        {
            context.Response.Clear();
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = apiException.StatusCode;
            var error = apiException.ToError();
            var errorJson = JsonSerializer.Serialize(error, error.GetType());
            await context.Response.WriteAsync(errorJson);
        }
    }
}
