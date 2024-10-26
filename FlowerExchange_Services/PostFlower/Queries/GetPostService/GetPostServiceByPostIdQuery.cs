using Domain.Entities;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.PostFlower.Queries.GetPostService
{
    public class GetPostServiceByPostIdQuery : IRequest<List<PostService>>
    {
        public Guid PostId { get; set; }
    }

    public class GetPostServiceByPostIdQueryHandle : IRequestHandler<GetPostServiceByPostIdQuery, List<PostService>>
    {
        private IPostServiceRepository _postServiceRepository;

        private readonly ILogger<GetPostServiceByPostIdQueryHandle> _logger;

        public GetPostServiceByPostIdQueryHandle(IPostServiceRepository postServiceRepository, ILogger<GetPostServiceByPostIdQueryHandle> logger)
        {
            _postServiceRepository = postServiceRepository;
            _logger = logger;
        }

        public async Task<List<PostService>> Handle(GetPostServiceByPostIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<PostService> list = await _postServiceRepository.GetByPostIdAsync(request.PostId);
                if (list == null)
                {
                    throw new NotFoundException("Post's services is null");
                }
                return list.ToList();
            }
            catch (NotFoundException)
            {
                throw new NotFoundException("Post not found");
            }
        }
    }
}
