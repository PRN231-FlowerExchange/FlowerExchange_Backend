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
        public PostViewDTO Post { get; set; }
        public List<ServiceViewDTO> ListService { get; set; }
        public int ServiceDay { get; set; }
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
            Entity.Post post = await _postRepository.GetByIdAsync(request.Post.Id);

            if (post == null)
            {
                throw new NotFoundException("Post not found");
            }

            double totalPrice = 0;
            List<PostService> listPostService = new List<PostService>();

            // Iterate through requested services
            foreach (ServiceViewDTO serviceDTO in request.ListService)
            {
                // Fetch the service entity
                Service serviceEntity = await _serviceRepository.GetByIdAsync(serviceDTO.Id);

                // Check if service exists before calculating total price
                if (serviceEntity == null)
                {
                    throw new NotFoundException("Service not found");
                }

                // Calculate the total price for the services
                totalPrice += serviceEntity.Price * request.ServiceDay;

                // Create a new PostService entry
                PostService postService = new PostService
                {
                    PostId = request.Post.Id,
                    Post = post,
                    ServiceId = serviceEntity.Id,
                    Service = serviceEntity,
                    CreatedAt = DateTime.UtcNow,
                    ExpiredAt = DateTime.UtcNow.AddDays(request.ServiceDay),
                };
                listPostService.Add(postService);
            }

            // Check if the user has enough money in the wallet (implement wallet logic)
            bool hasEnoughMoney = true; // Replace with actual wallet balance check
            if (!hasEnoughMoney)
            {
                throw new Exception("Not enough money to pay");
            }

            List<PostService> entityList = (List<PostService>)await _postServiceRepository.GetByPostIdAsync(request.Post.Id);
            if (entityList.Any())
            {
                await _postServiceRepository.DeleteRangeAsync(entityList);
            }

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
