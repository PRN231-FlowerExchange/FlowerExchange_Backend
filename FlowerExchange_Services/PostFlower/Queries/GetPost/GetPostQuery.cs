using Application.PostFlower.DTOs;
using Application.PostFlower.Services;
using Domain.Exceptions;
using Domain.Models;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.PostFlower.Queries.GetPost
{
    public class GetPostQuery : IRequest<List<PostViewDTO>>
    {
        //public Post? Post { get; set; }//filter entity
        public Guid StoreId { get; set; } = Guid.Empty; // Default to an empty GUID
        public Guid SellerId { get; set; } = Guid.Empty;
        public List<Guid>? Categories { get; set; } = null;// Default to an empty GUID
        public string SearchString { get; set; } = ""; // Default to null
        public PaginateRequest PaginateRequest { get; set; }
        public List<SortCriteria>? sortCriterias { get; set; } = null;
    }

    public class GetPostQueryHandler : IRequestHandler<GetPostQuery, List<PostViewDTO>>
    {
        private IPostRepository _postRepository;
        private ICategoriesRepository _cateRepository;

        private readonly ILogger<GetPostQueryHandler> _logger;

        public GetPostQueryHandler(IPostRepository postRepository, ICategoriesRepository cateRepository, ILogger<GetPostQueryHandler> logger)
        {
            _postRepository = postRepository;
            _cateRepository = cateRepository;
            _logger = logger;
        }

        public async Task<List<PostViewDTO>> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            List<PostViewDTO> result = new List<PostViewDTO>();
            Domain.Entities.Post postEntity = new Domain.Entities.Post()
            {
                StoreId = request.StoreId,
                SellerId = request.SellerId,
                PostCategories = new List<Domain.Entities.PostCategory>()
            };

            // Add Categories if provided
            if (request.Categories != null && request.Categories.Any())
            {
                request.Categories.ForEach(category =>
                {
                    postEntity.PostCategories.Add(new Domain.Entities.PostCategory { CategoryId = category });
                });
            }

            try
            {
                // Await the async call
                List<Domain.Entities.Post> listPost = (List<Domain.Entities.Post>)await _postRepository.GetPosts(postEntity, request.PaginateRequest.CurrentPage, request.PaginateRequest.PageSize, request.SearchString,
                    request.sortCriterias);

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
                            PostViewDTO viewDTO = ConvertFuction.ConvertObjectToObject<PostViewDTO, Domain.Entities.Post>(post);

                            List<Domain.Entities.Category> listCates = await _cateRepository.GetCategoryByPostId(post.Id);
                            viewDTO.Categories = ConvertFuction.ConvertListToList<PostCategoryDTO, Domain.Entities.Category>(listCates);

                            result.Add(viewDTO);
                        }
                        else
                        {
                            foreach (var service in post.PostServices)
                            {
                                PostViewDTO viewPost = ConvertFuction.ConvertObjectToObject<PostViewDTO, Domain.Entities.Post>(post);
                                List<Domain.Entities.Category> listCates = await _cateRepository.GetCategoryByPostId(post.Id);
                                viewPost.Categories = ConvertFuction.ConvertListToList<PostCategoryDTO, Domain.Entities.Category>(listCates);
                                if (service.ExpiredAt > DateTime.UtcNow)
                                {  
                                    viewPost.priority = 1;   
                                }
                                result.Add(viewPost);
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
