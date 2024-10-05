using Application.Post.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Models;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;

public class GetUserPostQuery : IRequest<PagedList<PostDTO>>
{
    public Guid UserId { get; set; }
    public PostParameters PostParameters { get; set; }
    public GetUserPostQuery(Guid userId, PostParameters postParameters = null)
    {
        UserId = userId; 
        PostParameters = postParameters;  

    }
}

public class GetUserPostQueryHandler : IRequestHandler<GetUserPostQuery, PagedList<PostDTO>>
{
    private Domain.Repository.IPostRepository _iPostRepository;

    private readonly ILogger<GetUserPostQueryHandler> _logger;

    private readonly IMapper _mapper;

    public GetUserPostQueryHandler(IPostRepository postRepository, ILogger<GetUserPostQueryHandler> logger, IMapper mapper)
    {
        _iPostRepository = postRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<PagedList<PostDTO>> Handle(GetUserPostQuery request, CancellationToken cancellationToken)
    {
        var posts = await _iPostRepository.GetPostsByUserIdAsync(request.UserId, request.PostParameters);

        if (posts == null || !posts.Any())
        {
            // Handle case when no posts are found for the user
            var errorMessage = $"No posts found for User ID: {request.UserId}.";
            _logger.LogWarning(errorMessage);
            throw new NotFoundException(errorMessage);
        }
        var response = _mapper.Map<PagedList<PostDTO>>(posts);
        return new PagedList<PostDTO>(response, posts.TotalCount, posts.CurrentPage, posts.PageSize);
    }
}
