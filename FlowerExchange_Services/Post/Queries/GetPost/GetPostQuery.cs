using Application.Post.DTOs;
using Application.Post.Services;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Post.Queries.GetPost
{
    public class GetPostQuery : IRequest<List<PostViewDTO>>
    {
        public Domain.Entities.Post? Post { get; set; }//filter entity
        public string SearchString { get; set; }
        public PaginateRequest PaginateRequest { get; set; }
    }

    public class GetPostQueryHandler : IRequestHandler<GetPostQuery, List<PostViewDTO>>
    {
        private IPostRepository _postRepository;

        private readonly ILogger<GetPostQueryHandler> _logger;

        public GetPostQueryHandler(IPostRepository postRepository, ILogger<GetPostQueryHandler> logger)
        {
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task<List<PostViewDTO>> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            List<PostViewDTO> result = new List<PostViewDTO>();

            try
            {
                // Await the async call
                List<Domain.Entities.Post> listPost = (List<Domain.Entities.Post>)await _postRepository.GetPosts(request.Post, request.PaginateRequest.CurrentPage, request.PaginateRequest.PageSize, request.SearchString);

                if (listPost == null || !listPost.Any())
                {
                    throw new Exception("No record match");
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
