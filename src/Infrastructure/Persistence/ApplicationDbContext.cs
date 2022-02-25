using Application.Common.Interfaces;
using Audit.Core;
using Domain.Common;
using Domain.Entities;
using Domain.Entities.Audit;
using Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        #region ENTITIES
        public DbSet<User> Users { get; set; }

        #endregion

        #region SAVING SUPPORT

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = "DefaultUser";
                        entry.Entity.Created = DateTime.Now;
                        entry.Entity.Active = true;
                        break;
                    case EntityState.Modified:
                        entry.Property("CreatedBy").IsModified = false;
                        entry.Property("Created").IsModified = false;
                        entry.Property("Active").CurrentValue = entry.Property("Active").CurrentValue ?? true;
                        entry.Entity.LastModifiedBy = "DefaultUser";
                        entry.Entity.LastModified = DateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public Task AddEntityToGraph<T>(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            this.ChangeTracker.TrackGraph(entity, e => e.Entry.State = e.Entry.IsKeySet ? EntityState.Modified : EntityState.Added);
            return Task.CompletedTask;
        }

        public override Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default)
        {
            return base.AddRangeAsync(entities, cancellationToken);
        }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Ignore<AuditEntryChange>();

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            OnModelCreatingPartial(builder);

            base.OnModelCreating(builder);
        }

        static void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.BaseType == null && typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    // disables concurrency check via RowVersion to avoid form locking and passing RowVersion via WebAPI for now
                    var entityTypeBuilder = modelBuilder.Entity(entityType.ClrType);
                    entityTypeBuilder.Property("RowVersion").IsConcurrencyToken(false);

                    // global query filter for all entities imlementing IEntityBase
                    // https://haacked.com/archive/2019/07/29/query-filter-by-interface/
                    // do not remove, if you need inactive entities, use IgnoreQueryFilters() instead
                    modelBuilder.SetEntityQueryFilter<AuditableEntity>(entityType.ClrType, p => p.Active == true);
                }

                foreach (var property in entityType.GetProperties())
                {
                    // ALL DATES IN THE DB MUST BE STORED AS UTC
                    // but datetime column types do not hold information about datetime kinds (e.g. local or UTC)
                    // this causes issues on the frontend/backend because new Date()/new DateTime() assumes a local time when left unspecified
                    // this value converter forces all entities to have a DateTime.Kind of Utc instead of Unspecified
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(ModelBuilderExtensions.DateTimeUtcConverter);
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(ModelBuilderExtensions.NullableDateTimeUtcConverter);
                    }
                }
            }
        }

        #region AUDIT

        public DbSet<UserAudit> UserAudits { get; set; }

        //public override void OnScopeCreated(IAuditScope auditScope)
        //{
        //    Database.BeginTransaction();
        //}

        //public override void OnScopeSaving(IAuditScope auditScope)
        //{
        //    try
        //    {
        //        // ... custom log saving ...
        //    }
        //    catch
        //    {
        //        // Rollback call is not mandatory. If exception thrown, the transaction won't get commited
        //        Database.CurrentTransaction.Rollback();
        //        throw;
        //    }
        //    Database.CurrentTransaction.Commit();
        //}

        #endregion
    }
}
