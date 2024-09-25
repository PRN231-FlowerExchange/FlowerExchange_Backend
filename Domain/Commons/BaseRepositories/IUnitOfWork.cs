using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Commons.BaseRepositories
{
    /// <summary>
    /// The unit of work class ensures that multiple repositories share a single database context,
    /// allowing for coordinated changes when the unit of work is completed.
    /// It includes a Save method and properties for each repository,
    /// with each repository instance being created using the same database context.
    /// This design guarantees that all related changes can be managed collectively through the SaveChanges method.
    /// </summary>
    public interface IUnitOfWork<out TContext> where TContext : DbContext, new()
    {
        public TContext Context { get; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        //DbContext Class SaveChanges method
        public void SaveChanges();

        //Start the database Transaction
        public void CreateTransaction();

        //Commit the database Transaction
        public void CommitChanges();

        //Rollback the database Transaction
        public void RollbackChanges();

    }
}
