using System.Linq;
using System.Threading.Tasks;
using ThaGet.Cqrs.Domain.Exceptions;
using ThaGet.Cqrs.Exceptions.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ThaGet.Cqrs.Domain.EntityFramework.Middleware
{
    public class ConcurrencyExceptionMiddleware : BaseExceptionMiddleware
    {
        public ConcurrencyExceptionMiddleware(RequestDelegate next, ILogger<BaseExceptionMiddleware> logger)
            : base(next, logger) { }

        public override async Task Invoke(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                LogAffectedEntities(concurrencyException);
                var apiException = new ConcurrencyException(concurrencyException);
                await HandleException(context, apiException);
            }
        }

        private void LogAffectedEntities(DbUpdateConcurrencyException concurrencyException)
        {
            var affectedEntities = concurrencyException.Entries.Select(x => new
            {
                entityType = x.Entity.GetType().Name,
                id = x.Property("Id").OriginalValue
            });

            var affectedEntitiesString = string.Join(", ", affectedEntities.Select(x => $"{x.entityType} with Id '{x.id}'"));

            Logger.LogError($"Concurrency Exception: {concurrencyException.Message}, Affected Entities: ({affectedEntitiesString})");
        }
    }
}
