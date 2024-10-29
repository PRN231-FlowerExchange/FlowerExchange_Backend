using Application.PostFlower.DTOs;
using AutoMapper;
using Domain.Commons.BaseRepositories;
using Domain.Constants.Enums;
using Domain.Repository;
using MediatR;
using Persistence;
using Persistence.RepositoryAdapter;
using DomainEntities = Domain.Entities; // Post bị tên namesapce nên đặt alias

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
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IPostCategoryRepository _postCategoryRepository;


        private readonly IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;

        public CreatePostCommandHandler(IMapper mapper,
            IPostRepository postRepository,
            IFlowerRepository flowerRepository,
            IPostCategoryRepository postCategoryRepository,
            ICategoriesRepository categoriesRepository , IUnitOfWork<FlowerExchangeDbContext> unitOfWork)
        {
            _mapper = mapper;
            _postRepository = postRepository;
            _categoriesRepository = categoriesRepository;
            _flowerRepository = flowerRepository;
            _postCategoryRepository = postCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Map từ DTO sang entity Post
                DomainEntities.Post newPost = _mapper.Map<DomainEntities.Post>(request.CreatePostDTO);
                newPost.PostStatus = PostStatus.Available;
                DomainEntities.Flower newFlower = _mapper.Map<DomainEntities.Flower>(request.CreatePostDTO.Flower);


                // Chuyển đổi ExpiredAt sang UTC
                request.CreatePostDTO.ExpiredAt = request.CreatePostDTO.ExpiredAt.ToUniversalTime();

                // Lấy danh mục từ database dựa trên ID được chọn
                var category = await _categoriesRepository.GetByIdAsync(request.CreatePostDTO.SelectedCategoryId);
                if (category == null)
                {
                    throw new Exception("Category not found");
                }
                // Gắn flower vào post
                newFlower.Post = newPost;
                newPost.Flower = newFlower;

                await _flowerRepository.InsertAsync(newFlower);
                await _postRepository.InsertAsync(newPost);
                var a = newPost.Id;
                var b = category.Id;
                // Tạo bản ghi trong bảng PostCategory để liên kết Post và Category
                var postCategory = new DomainEntities.PostCategory
                {
                    PostId = newPost.Id,
                    CategoryId = category.Id,
                };


                await _postCategoryRepository.InsertAsync(postCategory);
                await _unitOfWork.SaveChangesAsync();

                return newPost.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the post: " + ex.Message);
            }
        }
    }
}
