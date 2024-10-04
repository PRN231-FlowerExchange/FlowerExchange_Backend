﻿using Domain.Commons.BaseRepositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IPostRepository : IRepositoryBase<Post, Guid>
    {
        Task<List<Post>> GetPosts(Post entity, int currentPage, int pageSize, string? searchString = null);
    }
}