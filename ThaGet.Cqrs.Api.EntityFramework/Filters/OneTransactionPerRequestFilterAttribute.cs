using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ThaGet.Cqrs.Api.EntityFramework.Filters
{
    public class OneTransactionPerRequestFilterAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context == null)
                return;

            if (next == null)
                throw new ArgumentNullException($"Argument {nameof(next)} is null");

            var useReadonlyTransaction = context.HttpContext.Request.Method == HttpMethods.Get;

            // TODO Test if thsi line finds the context
            var dbContext = context.HttpContext.RequestServices.GetService<DbContext>();

            if (useReadonlyTransaction)
            {
                dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            }

            using var transaction = dbContext.Database.BeginTransaction();

            try
            {
                var executedContext = await next();
                if (executedContext.Exception != null)
                {
                    throw executedContext.Exception;
                }

                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw e;
            }
        }
    }
}
