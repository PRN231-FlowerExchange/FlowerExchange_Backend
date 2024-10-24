using Domain.Commons.BaseRepositories;
using Domain.Constants.Enums;
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.RepositoryAdapter
{
    public class PostServiceRepository : RepositoryBase<PostService, Guid>, IPostServiceRepository
    {
        private readonly FlowerExchangeDbContext _context;

        public PostServiceRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
            _context = unitOfWork.Context;
        }

        public async Task<List<PostService>> GetPostServicesByPostIdAndServiceIdsAndStatus(Guid postId, List<Guid> serviceIds, PostServiceStatus postServiceStatus)
        {
            var postServices = new List<PostService>();
            if(!await _context.PostServices.AnyAsync())
                return postServices;
            try
            {
                postServices = await _context.PostServices
                    .Include(ps => ps.Post)
                    .Include(ps => ps.Service)
                    .Where(ps => ps.PostId.Equals(postId))
                    .Where(ps => serviceIds.Contains(ps.ServiceId))
                    .Where(ps => ps.Status == postServiceStatus)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return postServices;
        }
    }
}
