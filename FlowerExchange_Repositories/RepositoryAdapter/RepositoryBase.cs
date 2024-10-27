using Domain.Commons.BaseEntities;
using Domain.Commons.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Validation;
using System.Linq.Expressions;

namespace Persistence.RepositoryAdapter
{
    public class RepositoryBase<TEntity, Tkey> : IRepositoryBase<TEntity, Tkey>, IDisposable where TEntity : class, IEntityWitkKey<Tkey>
    {
        protected readonly FlowerExchangeDbContext _dbContext;
        protected DbSet<TEntity> _dbSet;
        private bool _isDisposed;
        protected readonly IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;
        private string _errorMessage = string.Empty;
        private FlowerExchangeDbContext context;

        public RepositoryBase(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : this(unitOfWork.Context)
        {
            _dbContext = unitOfWork.Context;
            _dbSet = _dbContext.Set<TEntity>();
            _isDisposed = false;
            _unitOfWork = unitOfWork;
        }

        public RepositoryBase(FlowerExchangeDbContext context)
        {
            this.context = context;
        }

        protected virtual DbSet<TEntity> DbSet { get { return _dbSet ?? (_dbSet = _dbContext.Set<TEntity>()); } }


        public virtual async Task<TEntity> AnyAsync(Func<TEntity, bool> predicate)
        {
            return await Task.Run(() => _dbSet.Any(predicate) ? _dbSet.Find(predicate) : null);
        }

        public virtual async Task<int> CountAsync(Func<TEntity, bool> predicate)
        {
            return await Task.Run(() => _dbSet.Count(predicate));
        }

        public virtual async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }
                _dbSet.Remove(entity);
                //commented out call to SaveChanges as Context save changes will be called with Unit of work
                //_dbContext.SaveChanges(); /// SaveChanges should be called by UnitOfWork manually, so no need to call it here
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleUnitOfWorkException(dbEx);
                throw new Exception(_errorMessage, dbEx);
            }
        }

        public virtual async Task DeleteRangeAsync(IQueryable<TEntity> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException(nameof(entities));
                if (entities.Any())
                    _dbContext.RemoveRange(entities);

            }
            catch (DbEntityValidationException dbEx)
            {
                HandleUnitOfWorkException(dbEx);
                throw new Exception(_errorMessage, dbEx);
            }

        }

        public virtual async Task DeleteByIdAsync(object key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var entity = await _dbSet.FindAsync(key);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }

        public virtual void Dispose()
        {
            if (_dbContext != null)
                _dbContext.Dispose();
            _isDisposed = true;
        }

        public virtual async Task<IQueryable<TEntity>> FindAll(Func<TEntity, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            return await Task.Run(() => _dbSet.Where(predicate).AsQueryable());
        }


        public virtual IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return _dbSet.Where(predicate).AsQueryable();
        }


        public virtual async Task<TEntity> FindAsync(Func<TEntity, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return await Task.Run(() => _dbSet.SingleOrDefault(predicate));
        }

        public virtual async Task<TEntity> FindByConditionAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            // Use AsQueryable to ensure it remains an IQueryable
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IQueryable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            // Use AsQueryable to ensure it remains an IQueryable
            return _dbSet.Where(predicate);
        }

        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }


        public virtual async Task InsertAsync(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));
                await _dbSet.AddAsync(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleUnitOfWorkException(dbEx);
                throw new Exception(_errorMessage, dbEx);
            }
        }

        public virtual async Task InsertRangeAsync(IQueryable<TEntity> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException(nameof(entities));

                if (entities.Any())
                    await _dbContext.AddRangeAsync(entities);
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleUnitOfWorkException(dbEx);
                throw new Exception(_errorMessage, dbEx);
            }
        }

        public async Task SaveChagesAysnc()
        {
            try
            {
                //Calling DbContext Class SaveChanges method 
                _dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleUnitOfWorkException(dbEx);
                throw new Exception(_errorMessage, dbEx);
            }
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));
                // Ensure entity is attached and marked as modified
                var entityIdProperty = typeof(TEntity).GetProperty("Id");
                if (entityIdProperty == null)
                    throw new InvalidOperationException("Entity does not have an Id property.");

                var existingEntity = await _dbSet.FindAsync(entityIdProperty);
                if (existingEntity != null)
                {
                    _dbContext.Entry(existingEntity).State = EntityState.Modified;
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleUnitOfWorkException(dbEx);
                throw new Exception(_errorMessage, dbEx);
            }
        }

        public async Task UpdateByIdAsync(TEntity entity, object id)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var existingEntity = await _dbSet.FindAsync(id);
            try
            {
                if (existingEntity != null)
                {
                    _dbContext.Entry(existingEntity).State = EntityState.Modified;
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleUnitOfWorkException(dbEx);
                throw new Exception(_errorMessage, dbEx);
            }

        }

        public virtual async Task UpdateRangeAsync(IQueryable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            if (entities.Any())
            {
                _dbContext.UpdateRange(entities);
            }
        }

        private void HandleUnitOfWorkException(DbEntityValidationException dbEx)
        {
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    _errorMessage = _errorMessage + $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage} {Environment.NewLine}";
                }
            }
        }

        private Expression<Func<TEntity, bool>> ConvertToExpression(Func<TEntity, bool> predicate)
        {
            // Create a parameter expression for TEntity
            var parameter = Expression.Parameter(typeof(TEntity), "entity");

            // Create a body expression based on the predicate
            var body = Expression.Invoke(Expression.Constant(predicate), parameter);

            // Create the lambda expression
            return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
        }
        
        // Method to get all entities with includes
        public virtual async Task<IEnumerable<TEntity>> GetAllWithIncludesAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            // Apply each include expression to the query
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync();
        }

        // Method to get entities with includes and a predicate
        public virtual async Task<IEnumerable<TEntity>> GetAllWithIncludesAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            // Apply includes
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            // Apply the predicate for filtering
            return await query.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> FindAsyncWithIncludesAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            // Apply includes
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return await Task.Run(() => query.SingleOrDefault(predicate));
        }
    }

}
