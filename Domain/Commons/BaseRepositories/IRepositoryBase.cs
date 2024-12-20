﻿using Domain.Commons.BaseEntities;
using System.Linq.Expressions;

namespace Domain.Commons.BaseRepositories
{
    public interface IRepositoryBase<TEntity, Tkey> where TEntity : class, IEntityWitkKey<Tkey>
    {
        public Task<TEntity> FindAsync(Func<TEntity, bool> predicate);
        public Task<TEntity> FindByConditionAsync(Expression<Func<TEntity, bool>> predicate);
        public Task<TEntity> FindAsyncWithIncludesAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        public Task<IQueryable<TEntity>> FindAll(Func<TEntity, bool> predicate);

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);

        public Task<IEnumerable<TEntity>> GetAllAsync();
        public Task<TEntity> GetByIdAsync(object id);
        public Task<IQueryable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate);


        public Task DeleteRangeAsync(IQueryable<TEntity> entities);
        public Task DeleteAsync(TEntity entity);
        public Task DeleteByIdAsync(object key);

        public Task InsertAsync(TEntity entity);
        public Task InsertRangeAsync(IQueryable<TEntity> entities);

        public Task UpdateByIdAsync(TEntity entity, object id);
        public Task UpdateAsync(TEntity entity);
        public Task UpdateRangeAsync(IQueryable<TEntity> entities);

        public Task<TEntity> AnyAsync(Func<TEntity, bool> predicate);
        public Task<int> CountAsync(Func<TEntity, bool> predicate);
        public Task<int> CountAsync();
        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        public Task SaveChagesAysnc();

        public Task<IEnumerable<TEntity>> GetAllWithIncludesAsync(params Expression<Func<TEntity, object>>[] includes);

        public Task<IEnumerable<TEntity>> GetAllWithIncludesAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);



    }
}
