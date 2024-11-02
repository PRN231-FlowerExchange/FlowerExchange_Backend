using Application.PostFlower.DTOs;
using AutoMapper;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Post.Queries.GetDetailPost
{
    public class GetDetailPostQuery : IRequest<PostDTO>
    {
        public Guid Id { get; set; }
        public GetDetailPostQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetDetailPostQueryHandler : IRequestHandler<GetDetailPostQuery, PostDTO>
    {
        private Domain.Repository.IPostRepository _iPostRepository;

        private readonly ILogger<GetDetailPostQueryHandler> _logger;

        private readonly IMapper _mapper;

        public GetDetailPostQueryHandler(IPostRepository postRepository, ILogger<GetDetailPostQueryHandler> logger, IMapper mapper)
        {
            _iPostRepository = postRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PostDTO> Handle(GetDetailPostQuery request, CancellationToken cancellationToken)
        {
            var post = await _iPostRepository.GetPostsByIdAsync(request.Id);
            if (post == null)   
            {
                var errorMessage = $"Post with Id: {request.Id} was not found.";
                _logger.LogWarning(errorMessage);
                throw new NotFoundException(errorMessage);
            }
            var response = _mapper.Map<PostDTO>(post);
            return response;
        }
    }
}
