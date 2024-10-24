using Application.PostFlower.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Models;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Post.Queries.GetAllPost
{
    public class GetAllPostQuery : IRequest<PagedList<AllPostDTO>>
    {
        public PostParameters PostParameters { get; set; }
        public GetAllPostQuery(PostParameters postParameters = null)
        {
            PostParameters = postParameters;
        }


    }
    public class GetAllPostQueryHandler : IRequestHandler<GetAllPostQuery, PagedList<AllPostDTO>>
    {
        private Domain.Repository.IPostRepository _iPostRepository;

        private readonly ILogger<GetUserPostQueryHandler> _logger;

        private readonly IMapper _mapper;

        public GetAllPostQueryHandler(IPostRepository postRepository, ILogger<GetUserPostQueryHandler> logger, IMapper mapper)
        {
            _iPostRepository = postRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PagedList<AllPostDTO>> Handle(GetAllPostQuery request, CancellationToken cancellationToken)
        {
            var posts = await _iPostRepository.GetAllPostAsync(request.PostParameters);

            if (posts == null || !posts.Any())
            {
                var errorMessage = $"No posts found for User ID: {request}.";
                _logger.LogWarning(errorMessage);
                throw new NotFoundException(errorMessage);
            }
            var response = _mapper.Map<PagedList<AllPostDTO>>(posts);
            return new PagedList<AllPostDTO>(response, posts.TotalCount, posts.CurrentPage, posts.PageSize);
        }
    }

}
