using Domain.Commons.BaseRepositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IPostServiceRepository : IRepositoryBase<PostService, Guid>
    {
        Task<IEnumerable<PostService>> GetByPostIdAsync(Guid postId);
        Task InsertRangeAsync(List<PostService> listPostService);
    }
}
