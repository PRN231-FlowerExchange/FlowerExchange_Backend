using Domain.Commons.BaseRepositories;
using Domain.Entities;

namespace Domain.Repository
{
    public interface IPostCategoryRepository : IRepositoryBase<PostCategory, Guid>
    {
    }
}
