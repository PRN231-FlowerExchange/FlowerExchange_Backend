using CrossCuttingConcerns.Datetimes;
using Domain.Commons.BaseEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Interceptors
{
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        public AuditableEntityInterceptor(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData, 
            InterceptionResult<int> result, 
            CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Interceptor SavingChanges is called");
            UpdateEntities(eventData.Context);

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void UpdateEntities(DbContext? context)
        {
            Console.WriteLine("Interceptor SavingChanges is called");
            if (context == null) return;

            foreach (EntityEntry<ITrackable> entry in context.ChangeTracker.Entries<ITrackable>())
            {
                if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    var utcNow = DateTime.UtcNow;
                    if (entry.State == EntityState.Added)
                    {
                        //TODO CONFIG:
                        //entry.Entity.CreatedBy = _user.Id;
                        entry.Entity.CreatedAt = _dateTimeProvider.UtcNow;

                    }
                    //TODO CONFIG
                    //entry.Entity.UpdatedBy = _user.Id;
                    entry.Entity.UpdatedAt = _dateTimeProvider.UtcNow;
                }
            }
        }
    }
    public static class Extensions
    {
        public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
            entry.References.Any(r =>
                r.TargetEntry != null &&
                r.TargetEntry.Metadata.IsOwned() &&
                (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }
}
