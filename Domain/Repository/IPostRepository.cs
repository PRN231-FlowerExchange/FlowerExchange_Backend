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
        Task<List<Post>> GetPosts(Post entity, int currentPage, int pageSize, string? searchString = null, List<SortCriteria>? sortCriteria = null);

        Task<PagedList<Post>> GetPostsByUserIdAsync(Guid userId, PostParameters postParameters);
        Task<Post> GetPostsByIdAsync(Guid id);
        Task<List<Post>> GetTopActivePostsWithNonExpiredServices(Post entity, int currentPage, int pageSize, int top);
        Task<PagedList<Post>> GetAllPostAsync(PostParameters postParameters);
        
        Task<Post?> GetPostByIdWithFlowerAsync(Guid id);
    }
}
