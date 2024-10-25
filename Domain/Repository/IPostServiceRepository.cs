﻿using Domain.Commons.BaseRepositories;
using Domain.Constants.Enums;
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
        Task<List<PostService>> GetPostServicesByPostIdAndServiceIdsAndStatus(Guid postId, List<Guid> serviceIds, PostServiceStatus postServiceStatus);
        Task DeleteRangeAsync(List<PostService> entityList);
        Task<IEnumerable<PostService>> GetByPostIdAsync(Guid postId);
        Task InsertRangeAsync(List<PostService> listPostService);
        Task<PostService> GetAsync(Guid postId, Guid ServiceId);
    }
}
