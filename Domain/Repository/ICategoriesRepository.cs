using Domain.Commons.BaseRepositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface ICategoriesRepository : IRepositoryBase<Category, Guid>
    {
        Task<List<Category>> GetCategoryByPostId(Guid postId);
    }
}
