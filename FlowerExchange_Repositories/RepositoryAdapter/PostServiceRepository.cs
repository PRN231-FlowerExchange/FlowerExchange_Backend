using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;

namespace Persistence.RepositoryAdapter
{
    public class PostServiceRepository : RepositoryBase<PostService, Guid>, IPostServiceRepository
    {
        public PostServiceRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IEnumerable<PostService>> GetByPostIdAsync(Guid postId)
        {
            return _dbContext.PostServices.Where(p => p.PostId.Equals(postId));
        }
    }
}
