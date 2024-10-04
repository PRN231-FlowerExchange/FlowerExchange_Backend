using Domain.Commons.BaseRepositories;
using Domain.Constants.Enums;
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
                .Include(p => p.PostStatus)
                .Include(p => p.StoreId)
                .Include(p => p.SellerId)
                .Include(p => p.Flower)
                .Where(p => (string.IsNullOrEmpty(searchString) || p.Title.Contains(searchString) || p.Description.Contains(searchString) || p.Flower.Name.Contains(searchString) || p.Store.Name.Contains(searchString) ))
                .Where(p => p.SellerId.Equals(entity.SellerId))
                .Where(p => p.Store.Equals(entity.StoreId))
                .Where(p => p.PostStatus.Equals(PostStatus.Available))
                .Include(p => p.PostServices).ThenInclude(s => s.Service)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return result;
        }
    }
}
