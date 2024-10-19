using Application.PostFlower.DTOs;
using Application.PostFlower.Services;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.PostFlower.Queries.GetPost
{
    public class GetTopPostQuery : IRequest<List<PostViewDTO>>
    {
        public int Top {  get; set; }
        public Guid StoreId { get; set; } = Guid.Empty;
        public Guid SellerId { get; set; } = Guid.Empty;
        public PaginateRequest PaginateRequest { get; set; }
    }

    public class GetTopPostQueryHandler : IRequestHandler<GetTopPostQuery, List<PostViewDTO>>
    {
        private IPostRepository _postRepository;

        private readonly ILogger<GetTopPostQueryHandler> _logger;

        public GetTopPostQueryHandler(IPostRepository postRepository, ILogger<GetTopPostQueryHandler> logger)
        {
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task<List<PostViewDTO>> Handle(GetTopPostQuery request, CancellationToken cancellationToken)
        {
            List<PostViewDTO> result = new List<PostViewDTO>();
            Domain.Entities.Post postEntity = new Domain.Entities.Post()
            {
                StoreId = request.StoreId,
                SellerId = request.SellerId,
            };
            try
            {
                // Await the async call
                List<Domain.Entities.Post> listPost = (List<Domain.Entities.Post>)await _postRepository.GetTopActivePostsWithNonExpiredServices(postEntity, request.PaginateRequest.CurrentPage, request.PaginateRequest.PageSize,request.Top);

                if (listPost == null || !listPost.Any())
                {
                    throw new NotFoundException("No record match");
                }
                else
                {
                    // Correct the generic types in the conversion function
                    foreach (var post in listPost)
                    {
                        if (post.PostServices == null || !post.PostServices.Any())
                        {
                            result.Add(ConvertFuction.ConvertObjectToObject<PostViewDTO, Domain.Entities.Post>(post));
                        }
                        else
                        {
                            foreach (var service in post.PostServices)
                            {
                                if (service.ExpiredAt > DateTime.UtcNow)
                                {
                                    PostViewDTO viewPost = ConvertFuction.ConvertObjectToObject<PostViewDTO, Domain.Entities.Post>(post);
                                    viewPost.priority = 1;
                                    result.Add(viewPost);
                                }
                            }
                        }
                        result.Sort((x, y) => x.priority.CompareTo(y.priority));
                    }

                }
            }
            catch (NotFoundException nfx)
            {
                throw nfx;
            }
            catch (Exception ex)
            {
                // Consider logging the exception or throwing a more descriptive error
                throw new Exception("Error occurred while fetching posts", ex);
            }

            // Return the result wrapped in a Task
            return result;
        }
    }
}
