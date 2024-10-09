using Application.Weather.Queries.GetWeather;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Post.Queries.GetDetailPost
{
    public class GetDetailPostQuery : IRequest<Domain.Entities.Post>
    {
        public Guid Id { get; set; }
        public GetDetailPostQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetDetailPostQueryHandler : IRequestHandler<GetDetailPostQuery, Domain.Entities.Post>
    {
        private Domain.Repository.IPostRepository _iPostRepository;

        private readonly ILogger<GetDetailPostQueryHandler> _logger;

        public GetDetailPostQueryHandler(IPostRepository postRepository, ILogger<GetDetailPostQueryHandler> logger)
        {
            _iPostRepository = postRepository;
            _logger = logger;
        }

        public async Task<Domain.Entities.Post> Handle(GetDetailPostQuery request, CancellationToken cancellationToken)
        {
            var post = await _iPostRepository.GetByIdAsync(request.Id);
            if (post == null)
            {
                // Handle case when post is not found
                var errorMessage = $"Post with Id: {request.Id} was not found.";
                _logger.LogWarning(errorMessage);
                throw new NotFoundException(errorMessage);  // Sử dụng NotFoundException đã sửa đổi
            }
            return post;
        }
    }
}
