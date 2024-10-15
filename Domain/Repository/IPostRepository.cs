using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IPostRepository : IRepositoryBase<Post, Guid>
    {
        Task<PagedList<Post>> GetPostsByUserIdAsync(Guid userId, PostParameters postParameters);
        Task<Post> GetPostsByIdAsync(Guid id);
    }
}
