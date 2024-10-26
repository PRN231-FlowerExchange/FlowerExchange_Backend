using Application.PostFlower.DTOs;
using Application.PostFlower.Services;
using AutoMapper;
using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using Persistence;
using Entity = Domain.Entities;

namespace Application.PostFlower.Commands.AddServiceToPostCommand
{
    public class AddServiceToPostCommand : IRequest<PostViewDTO>
    {
        public Guid PostId { get; set; }
        public List<Guid> ListServices { get; set; }
    }

    public class AddServicePostCommandHandle : IRequestHandler<AddServiceToPostCommand, PostViewDTO>
    {
        private IMapper _mapper;
        private IServiceRepository _serviceRepository;
        private IPostRepository _postRepository;
        private IPostServiceRepository _postServiceRepository;
        private IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;

        public AddServicePostCommandHandle(
            IMapper mapper,
            IServiceRepository serviceRepository,
            IPostRepository postRepository,
            IPostServiceRepository postServiceRepository,
            IUnitOfWork<FlowerExchangeDbContext> unitOfWork)
        {
            _mapper = mapper;
            _serviceRepository = serviceRepository;
            _postRepository = postRepository;
            _postServiceRepository = postServiceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PostViewDTO> Handle(AddServiceToPostCommand request, CancellationToken cancellationToken)
        {
            // Fetch the post
            Entity.Post post = await _postRepository.GetByIdAsync(request.PostId);

            if (post == null)
            {
                throw new NotFoundException("Post not found");
            }

            List<PostService> listPostService = new List<PostService>();

            // Iterate through requested services
            foreach (Guid serviceDTO in request.ListServices)
            {
                // Fetch the service entity
                Service serviceEntity = await _serviceRepository.GetByIdAsync(serviceDTO);

                // Check if service exists before calculating total price
                if (serviceEntity == null)
                {
                    throw new NotFoundException("Service not found");
                }

                PostService entityPostService = await _postServiceRepository.GetAsync(post.Id, serviceEntity.Id);

                if (entityPostService != null)
                {
                    throw new DuplicateWaitObjectException("PostService already existed");
                }

                // Create a new PostService entry
                PostService postService = new PostService
                {
                    PostId = request.PostId,
                    Post = post,
                    ServiceId = serviceEntity.Id,
                    Service = serviceEntity,
                    ServiceOrderId = Guid.Empty,
                    CreatedAt = DateTime.UtcNow,
                    ExpiredAt = DateTime.UtcNow,
                };
                listPostService.Add(postService);   
            }

            //List<PostService> entityList = (List<PostService>) await _postServiceRepository.GetByPostIdAsync(request.PostId);
            //if (entityList.Any())
            //{
            //    await _postServiceRepository.DeleteRangeAsync(entityList);
            //}

            if (listPostService == null || !listPostService.Any())
            {
                throw new Exception("No services to insert for the post.");
            }

            await _postServiceRepository.InsertRangeAsync(listPostService);
            await _unitOfWork.SaveChangesAsync();

            // Return the updated post as a DTO
            return ConvertPostToView(await _postRepository.GetByIdAsync(post.Id));
        }

        private PostViewDTO ConvertPostToView(Entity.Post source)
        {
            return ConvertFuction.ConvertObjectToObject<PostViewDTO, Entity.Post>(source);
        }

    }
}
