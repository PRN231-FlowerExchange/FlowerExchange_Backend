using AutoMapper;
using Domain.Commons.BaseRepositories;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Domain.Repository;
using DomainEntities = Domain.Entities; // Post bị tên namesapce nên đặt alias
using Persistence;
using Application.PostFlower.DTOs;

namespace Application.Post.Commands.CreatePost
{
    public class CreatePostCommand : IRequest<Guid>
    {
        public CreatePostDTO CreatePostDTO { get; init; }

        public CreatePostCommand(CreatePostDTO createPostDTO)
        {
            CreatePostDTO = createPostDTO;
        }
    }

    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IPostRepository _postRepository;
        private readonly IFlowerRepository _flowerRepository;
        private readonly IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;

        public CreatePostCommandHandler(IMapper mapper, IPostRepository postRepository, IFlowerRepository flowerRepository, IUnitOfWork<FlowerExchangeDbContext> unitOfWork)
        {
            _mapper = mapper;
            _postRepository = postRepository;
            _flowerRepository = flowerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            // Map từ DTO sang entity Post
            DomainEntities.Post newPost = _mapper.Map<DomainEntities.Post>(request.CreatePostDTO);

            DomainEntities.Flower newFlower = _mapper.Map<DomainEntities.Flower>(request.CreatePostDTO.Flower);
            // Gắn flower vào post
            newFlower.Post = newPost; 

            // Thêm mới post và flower vào database
            await _postRepository.InsertAsync(newPost);
            await _flowerRepository.InsertAsync(newFlower);


            await _unitOfWork.SaveChangesAsync();

            return newPost.Id;
        }
    }
}
