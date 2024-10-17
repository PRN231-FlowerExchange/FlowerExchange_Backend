using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Models;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.RepositoryAdapter
{
    public class PostRepository : RepositoryBase<Post, Guid>, IPostRepository
    {
        private readonly FlowerExchangeDbContext _context;

        public PostRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
            _context = unitOfWork.Context; 
        }

        public async Task<PagedList<Post>> GetPostsByUserIdAsync(Guid userId, PostParameters postParameters)
        {
            try
            {
                var query = _context.Posts
                    .Include(c => c.Store)
                    .Include(c => c.Flower)
                    .Where(post => post.StoreId == userId);

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
                return _context.Posts
                    .Include(c => c.Store)
                    .Include(c => c.Flower)
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
                var query = _context.Posts
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
                posts = posts.OrderBy(x => x.CreatedAt);
                return;
            }

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Post).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                    continue;

                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                posts = posts.OrderBy(x => x.CreatedAt);
                return;
            }
            posts = posts.OrderBy(orderQuery);
        }
    }
}
