using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using CrossCuttingConcerns.Utils;
using Domain.Models;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using Domain.Exceptions;

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

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _dbContext.Categories
                .AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task<Category> GetCategoryById(Guid id)
        {
            // Truy vấn trực tiếp từ DbSet mà không sử dụng các hàm sẵn
            var category = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == id); 

            // Kiểm tra xem danh mục có tồn tại không
            if (category == null)
            {
                throw new NotFoundException("Category not found");
            }

            return category;
        }





    }
}
