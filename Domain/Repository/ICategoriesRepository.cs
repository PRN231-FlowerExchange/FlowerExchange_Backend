using Domain.Commons.BaseRepositories;
using Domain.Entities;

namespace Domain.Repository
{
    public interface ICategoriesRepository : IRepositoryBase<Category, Guid>
    {
        Task<List<Category>> GetCategoryByPostId(Guid postId);
        Task<bool> ExistsByNameAsync(string name);
        Task<Category> GetCategoryById(Guid id);
    }
}
