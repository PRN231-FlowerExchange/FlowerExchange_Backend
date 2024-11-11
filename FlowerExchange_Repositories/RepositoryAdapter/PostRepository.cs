using CrossCuttingConcerns.Utils;
using Domain.Commons.BaseRepositories;
using Domain.Constants.Enums;
using Domain.Entities;
using Domain.Models;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace Persistence.RepositoryAdapter
{
    public class PostRepossitory : RepositoryBase<Post, Guid>, IPostRepository
    {

        public PostRepossitory(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<Post>> GetPosts(Post entity,
                                               int currentPage,
                                               int pageSize,
                                               string? searchString = null,
                                               List<SortCriteria>? sortCriteria = null)
        {
            var query = _dbContext.Posts
                .Include(p => p.Store)
                .Include(p => p.Seller)
                .Include(p => p.Flower)
                .Include(p => p.PostCategories)
                .Include(p => p.PostServices).ThenInclude(s => s.Service)
                .Where(p => p.PostStatus ==  PostStatus.Available)
                // Apply SellerId filter only if SellerId is provided
                .Where(p => entity.SellerId == Guid.Empty || p.SellerId == entity.SellerId)
                // Apply StoreId filter only if StoreId is provided
                .Where(p => entity.StoreId == Guid.Empty || p.StoreId == entity.StoreId)
                // Apply search filter if provided
            .   Where(p => (string.IsNullOrEmpty(searchString)
                        || p.Title.ToLower().Contains(searchString.Trim().ToLower())
                        || p.Description.ToLower().Contains(searchString.Trim().ToLower())
                        || p.Flower.Name.ToLower().Contains(searchString.Trim().ToLower())
                        || p.Store.Name.ToLower().Contains(searchString.Trim().ToLower())));


            if (entity.PostCategories.Any())
            {
                foreach (var category in entity.PostCategories)
                {
                    query = query.Where(p => p.PostCategories.Any(pc => pc.CategoryId == category.CategoryId));
                }
            }

            if (sortCriteria != null && sortCriteria.Any())
            {
                IOrderedQueryable<Post>? orderedQuery = null;

                for (int i = 0; i < sortCriteria.Count; i++)
                {
                    var criteria = sortCriteria[i];
                    if (i == 0)
                    {
                        orderedQuery = query.OrderByDynamic(criteria.SortBy, criteria.IsDescending);
                    }
                    else
                    {
                        orderedQuery = orderedQuery.ThenByDynamic(criteria.SortBy, criteria.IsDescending);
                    }
                }

                query = orderedQuery ?? query;
            }

            query = query.Skip((currentPage - 1) * pageSize)
                             .Take(pageSize);

            // Execute the query and return the result  
            return await query.ToListAsync();
        }

        public async Task<List<Post>> GetTopActivePostsWithNonExpiredServices(Post entity, int currentPage, int pageSize, int top)
        {
            var now = DateTime.Now.ToUniversalTime();

            var query = _dbContext.Posts
                .Include(p => p.Store)
                .Include(p => p.Seller)
                .Include(p => p.Flower)
                .Include(p => p.PostServices).ThenInclude(s => s.Service)
                .Where(p => p.PostStatus == PostStatus.SoldOut)
                // Apply SellerId filter only if SellerId is provided
                .Where(p => entity.SellerId == Guid.Empty || p.SellerId == entity.SellerId)
                // Apply StoreId filter only if StoreId is provided
                .Where(p => p.PostServices.Any(ps => ps.ExpiredAt > now))
                .OrderBy(p => p.CreatedAt)
                .OrderByDescending(p => p.ExpiredAt)
                .Take(top);
            return await query.ToListAsync();
        }

        public async Task<PagedList<Post>> GetPostsByUserIdAsync(Guid userId, PostParameters postParameters)
        {
            try
            {
                var query = _dbContext.Posts
                    .Include(c => c.Store)
                    .Include(c => c.Flower)
                    .Include(c => c.Seller)
                    .Where(post => post.StoreId == null ? post.SellerId == userId : post.StoreId == userId);

                SearchByName(ref query, postParameters.Title);
                ApplySort(ref query, postParameters.OrderBy);

                return await PagedList<Post>.ToPagedList(query, postParameters.PageNumber, postParameters.PageSize);
            }
            catch (Exception ex)
            {
                // Log the exception here if necessary
                throw;
            }
        }

        public async Task<Post> GetPostsByIdAsync(Guid id)
        {
            try
            {
                return _dbContext.Posts
                    .Include(c => c.Store)
                    .Include(c => c.Flower)
                    .Include(c => c.Seller)
                    .Include(c => c.PostCategories).ThenInclude(c => c.Category)
                    .FirstOrDefault(post => post.Id == id);
            }
            catch (Exception ex)
            {
                // Log the exception here if necessary
                throw;
            }
        }

        public async Task<PagedList<Post>> GetAllPostAsync(PostParameters postParameters)
        {
            try
            {
                var query = _dbContext.Posts
                    .Include(c => c.Store)
                    .Include(c => c.Flower).AsNoTracking();

                SearchByName(ref query, postParameters.Title);
                ApplySort(ref query, postParameters.OrderBy);

                return await PagedList<Post>.ToPagedList(query, postParameters.PageNumber, postParameters.PageSize);
            }
            catch (Exception ex)
            {
                // Log the exception here if necessary
                throw;
            }
        }

        private void SearchByName(ref IQueryable<Post> posts, string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return;

            posts = posts.Where(o => o.Title.ToLower().Contains(title.Trim().ToLower()));
        }

        private void ApplySort(ref IQueryable<Post> posts, string orderByQueryString)
        {
            if (!posts.Any())
                return;

            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                posts = posts.OrderBy(x => x.Flower.Price); 
                return;
            }

            var orderParams = orderByQueryString.Trim().Split(',');
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(" ")[0];
                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";

                if (propertyFromQueryName.Equals("price", StringComparison.InvariantCultureIgnoreCase))
                {
                    orderQueryBuilder.Append($"Flower.Price {sortingOrder}, ");
                }
                else if (propertyFromQueryName.Equals("storeName", StringComparison.InvariantCultureIgnoreCase))
                {
                    orderQueryBuilder.Append($"Store.Name {sortingOrder}, ");
                }
                else
                {
                    var propertyInfos = typeof(Post).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                    if (objectProperty != null)
                    {
                        orderQueryBuilder.Append($"{objectProperty.Name} {sortingOrder}, ");
                    }
                }
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                posts = posts.OrderBy(x => x.Flower.Price);
            }
            else
            {
                posts = posts.OrderBy(orderQuery);
            }
        }
        
        public async Task<Post?> GetPostByIdWithFlowerAsync(Guid id)
        {
            try
            {
                return await _dbContext.Posts
                    .Include(p => p.Flower)
                    .Include(p => p.Seller)
                    .FirstOrDefaultAsync(p => p.Id.Equals(id));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}