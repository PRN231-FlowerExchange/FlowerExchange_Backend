using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;


namespace Persistence.RepositoryAdapter
{
    public class PostCategoryRepository : RepositoryBase<PostCategory, Guid>, IPostCategoryRepository
    {
        public PostCategoryRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
