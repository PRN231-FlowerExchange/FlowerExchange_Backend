using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;
using System.Data.Entity.Validation;

namespace Persistence.RepositoryAdapter
{
    public class PostServiceRepository : RepositoryBase<PostService, Guid>, IPostServiceRepository
    {
        public PostServiceRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
        }

        public async Task DeleteRangeAsync(List<PostService> entityList)
        {
            try
            {
                if (entityList == null)
                    throw new ArgumentNullException(nameof(entityList));
                if (entityList.Any())
                    _dbContext.RemoveRange(entityList);

            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception("Fail", dbEx);
            }
        }

        public async Task<IEnumerable<PostService>> GetByPostIdAsync(Guid postId)
        {
            return _dbContext.PostServices.Where(p => p.PostId.Equals(postId));
        }

        public async Task InsertRangeAsync(List<PostService> listPostService)
        {
            try
            {
                if (listPostService == null)
                    throw new ArgumentNullException(nameof(listPostService));

                if (listPostService.Any())
                    await _dbContext.PostServices.AddRangeAsync(listPostService);
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception("Add fail", dbEx);
            }
        }

        public async Task<PostService> GetAsync(Guid postId, Guid ServiceId)
        {
            return _dbContext.PostServices.Where(p => p.PostId.Equals(postId)).Where(p=>p.ServiceId.Equals(ServiceId)).FirstOrDefault();
        }
    }
}
