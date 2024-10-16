﻿using Domain.Commons.BaseRepositories;
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

        public async Task<List<Post>> GetPosts(Post entity, int currentPage, int pageSize, string? searchString = null)
        {
            var query = _dbContext.Posts
                .Include(p => p.PostStatus)
                .Include(p => p.Store)
                .Include(p => p.Seller)
                .Include(p => p.Flower)
                .Include(p => p.PostServices).ThenInclude(s => s.Service)
                .Where(p => p.SellerId == entity.SellerId && p.StoreId == entity.StoreId && p.PostStatus == PostStatus.Available);

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => p.Title.ToLower().Contains(searchString.Trim().ToLower())
                    || p.Description.ToLower().Contains(searchString.Trim().ToLower())
                    || p.Flower.Name.ToLower().Contains(searchString.Trim().ToLower())
                    || p.Store.Name.ToLower().Contains(searchString.Trim().ToLower()));
            }

            // Pagination
            query = query.Skip((currentPage - 1) * pageSize)
                         .Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<List<Post>> GetTopActivePostsWithNonExpiredServices(Post entity, int currentPage, int pageSize, int top, string? searchString = null)
        {
            var now = DateTime.Now;

            var query = _dbContext.Posts
                .Include(p => p.PostStatus)
                .Include(p => p.Store)
                .Include(p => p.Seller)
                .Include(p => p.Flower)
                .Include(p => p.PostServices).ThenInclude(s => s.Service)
                .Where(p => p.SellerId == entity.SellerId && p.StoreId == entity.StoreId && p.PostStatus == PostStatus.Available);

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => p.Title.ToLower().Contains(searchString.Trim().ToLower())
                    || p.Description.ToLower().Contains(searchString.Trim().ToLower())
                    || p.Flower.Name.ToLower().Contains(searchString.Trim().ToLower())
                    || p.Store.Name.ToLower().Contains(searchString.Trim().ToLower()));
            }

            query.Where(p => p.PostServices.Any(ps => ps.ExpiredAt > now))
                .OrderBy(p => p.CreatedAt)
                .OrderByDescending(p => p.ExpiredAt);


            return await query.Take(top).ToListAsync();
        }

        public async Task<PagedList<Post>> GetPostsByUserIdAsync(Guid userId, PostParameters postParameters)
        {
            try
            {
                var query = _dbContext.Posts
                    .Include(c => c.Store)
                    //.Include(c => c.Flower)
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
                return _dbContext.Posts
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