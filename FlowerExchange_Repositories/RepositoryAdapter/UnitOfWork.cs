using Domain.Commons.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Entity.Validation;


namespace Persistence.RepositoryAdapter
{
    /// <summary>
    /// Implement the UnitOfWork Interface/class which includes methods
    /// for starting, committing, and rolling back transaction.
    /// It also tracks changes made to entities during the transaction
    /// </summary>
    public sealed class UnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable where TContext : DbContext, new()
    {
        private readonly TContext _dbContext;
        private IDbContextTransaction _currentTransaction;
        private bool _disposed;
        private string _errorMessage = string.Empty;



        // Initialize the DbContext through dependency injection
        public UnitOfWork() //inject FlowerExchangeDbContext
        {
            _dbContext = new TContext();
        }

        public TContext Context => _dbContext;

        // Used to create a database transaction to do database operations
        public void CreateTransaction()
        {
            _currentTransaction = _dbContext.Database.BeginTransaction();
        }

        // If all the transactions complete successfully, call CommitChanges()
        // to save the changes permanently in the database
        public void CommitChanges()
        {
            _currentTransaction?.Commit();
        }

        // If at least one of the transactions fails, call RollbackChanges()
        // to rollback the database changes to their previous state
        public void RollbackChanges()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                // Ensure EF is no longer using that transaction and clean up the transaction object
                _currentTransaction?.Dispose();
            }
        }

        // The SaveChanges method implements the DbContext class's SaveChanges method.
        // Whenever a transaction is completed, call SaveChanges() to persist changes in the database permanently
        public void SaveChanges()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                _errorMessage = BuildErrorMessage(dbEx);
                throw new Exception(_errorMessage, dbEx);
            }
        }

        // Asynchronously save changes to the database, with cancellation support
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbEntityValidationException dbEx)
            {
                _errorMessage = BuildErrorMessage(dbEx);
                throw new Exception(_errorMessage, dbEx);
            }
        }

        // Build a detailed error message from DbEntityValidationException
        private static string BuildErrorMessage(DbEntityValidationException dbEx)
        {
            var errorMessages = dbEx.EntityValidationErrors
                .SelectMany(validationErrors => validationErrors.ValidationErrors)
                .Select(validationError => $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");

            return string.Join(Environment.NewLine, errorMessages);
        }

        // Disposing of the context object
        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                    _currentTransaction?.Dispose();
                }
                _disposed = true;
            }
        }

        // Used to free unmanaged resources
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
