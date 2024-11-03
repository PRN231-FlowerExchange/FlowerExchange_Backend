using Domain.Commons.BaseRepositories;
using Domain.Constants.Enums;
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Validation;

namespace Persistence.RepositoryAdapter
{
    public class PostServiceRepository : RepositoryBase<PostService, Guid>, IPostServiceRepository
    {

        public PostServiceRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<PostService>> GetPostServicesByPostIdAndServiceIdsAndStatus(Guid postId, List<Guid> serviceIds, PostServiceStatus postServiceStatus)
        {
            var postServices = new List<PostService>();
            if (!await _dbContext.PostServices.AnyAsync())
                return postServices;
            try
            {
                postServices = await _dbContext.PostServices
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

        public async Task DeleteRangeAsync(List<PostService> entityList)
        {
            try
            {
                if (entityList == null)
                    throw new ArgumentNullException(nameof(entityList));
                if (entityList.Any())
                    _dbContext.RemoveRange(entityList);

            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception("Fail", dbEx);
            }
        }

        public async Task<IEnumerable<PostService>> GetByPostIdAsync(Guid postId)
        {
            return _dbContext.PostServices.Where(p => p.PostId.Equals(postId));
        }

        public async Task InsertRangeAsync(List<PostService> listPostService)
        {
            try
            {
                if (listPostService == null)
                    throw new ArgumentNullException(nameof(listPostService));

                if (listPostService.Any())
                    await _dbContext.PostServices.AddRangeAsync(listPostService);
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception("Add fail", dbEx);
            }
        }

        public async Task<PostService> GetAsync(Guid postId, Guid ServiceId)
        {
            return _dbContext.PostServices.Where(p => p.PostId.Equals(postId)).Where(p=>p.ServiceId.Equals(ServiceId)).FirstOrDefault();
        }
    }
}
