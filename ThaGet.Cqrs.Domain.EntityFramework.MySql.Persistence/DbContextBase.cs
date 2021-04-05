using Microsoft.EntityFrameworkCore;
using System;

namespace ThaGet.Cqrs.Domain.EntityFramework.MySql.Persistence
{
    public abstract class DbContext<TDbContext, TId> : EntityFramework.Persistence.DbContextBase<TDbContext, TId, DateTime>
        where TDbContext : DbContext
        where TId : struct
    {
        protected DbContext(DbContextOptions<TDbContext> options)
            : base(options)
        {
        }

        protected override void BeforeBuild(ModelBuilder modelBuilder)
        {
        }
    }
}
