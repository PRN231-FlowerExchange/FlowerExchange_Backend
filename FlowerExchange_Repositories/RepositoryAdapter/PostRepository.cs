using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Persistence.RepositoryAdapter
{
    public class PostRepossitory : RepositoryBase<Post, Guid>, IPostRepository
    {

        public PostRepossitory(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<Post>> GetPosts(Post entity, int currentPage, int pageSize, string? searchString = null)
        {
            List<Post> result = new();

            result = await _dbContext.Posts
                .Where(p => (string.IsNullOrEmpty(searchString) || p.Title.Contains(searchString) || p.Description.Contains(searchString)))
                .Include(p => p.PostStatus)
                .Include(p => p.PostServices).ThenInclude(s => s.Service)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return result;
        }
    }
}
