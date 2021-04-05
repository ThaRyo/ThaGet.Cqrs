using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ThaGet.Cqrs.Domain.Entities;
using ThaGet.Cqrs.Domain.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using ThaGet.Shared;
using ThaGet.Cqrs.Domain.EntityFramework.Abstractions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ThaGet.Cqrs.Domain.EntityFramework.Persistence
{
    public abstract class DbContextBase<TDbContext, TId, TRowVersion> : DbContext, IUnitOfWork, IDbContextBase
        where TDbContext : DbContext
        where TId : struct
    {
        public static string EntitySchema;

        protected DbContextBase(DbContextOptions<TDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Saves all entries for which changes are detected
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> SaveAllChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();
            await SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Saves all entries with entity state added or modified
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var modifiedEntries = ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified).ToList();

            foreach (var entry in modifiedEntries)
            {
                if (!(entry.Entity is EntityBase<TId>))
                    break;

                if (entry.Entity is TrackedEntityBase<TId, TRowVersion> trackedEntity)
                {
                    var entity = (EntityBase<TId>)entry.Entity;

                    if (entry.State == EntityState.Added)
                        trackedEntity.CreatedAtUtc = DateTime.UtcNow;

                    trackedEntity.UpdatedAtUtc = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            BeforeBuild(modelBuilder);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
                modelBuilder.Entity(entity.Name).ToTable(entity.Name);

            AfterBuild(modelBuilder);

            ModelCreator.ConfigureEntities(modelBuilder);
            ModelCreator.SpecifyAllDateTimeToUtc(modelBuilder);
        }

        protected virtual void BeforeBuild(ModelBuilder modelBuilder)
        {
            ArgumentHelper.ThrowIfNullOrEmpty(EntitySchema, nameof(EntitySchema), "No schema provided");
            modelBuilder.HasDefaultSchema(EntitySchema);
        }

        protected virtual void AfterBuild(ModelBuilder modelBuilder)
        {
        }
    }
}
