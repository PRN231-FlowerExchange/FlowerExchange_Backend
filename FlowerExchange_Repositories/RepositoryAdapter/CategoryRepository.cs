using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using CrossCuttingConcerns.Utils;
using Domain.Models;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace Persistence.RepositoryAdapter
{
    public class CategoryRepository : RepositoryBase<Category, Guid>, ICategoriesRepository
    {
        public CategoryRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<Category>> GetCategoryByPostId(Guid postId)
        {
            var query = await _dbContext.Categories
                .Include(c => c.PostCategories)
                .ThenInclude(pc => pc.Post)
                .Where(c => c.PostCategories.Any(pc => pc.PostId == postId))
                .ToListAsync();

            return query;
        }
    }
}
