using Application.PostFlower.DTOs;
using Application.PostFlower.Services;
using AutoMapper;
using Domain.Commons.BaseRepositories;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.PostFlower.Commands.UpdatePostCommand
{
    public record UpdatePostCommand : IRequest<PostUpdateDTO>
    {
        public PostUpdateDTO UpdatePost { get; init; } = default;
    }

    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, PostUpdateDTO>
    {
        private readonly IMapper _mapper;
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;

        public UpdatePostCommandHandler(IMapper mapper, IPostRepository postRepository, IUnitOfWork<FlowerExchangeDbContext> unitOfWork)
        {
            _mapper = mapper;
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PostUpdateDTO> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var postEntity = await _postRepository.GetByIdAsync(request.UpdatePost.Id);
                if (postEntity == null)
                {
                    throw new NotFoundException("Post not found");
                }

                // Chuyển đổi ExpiredAt sang UTC 
                if (request.UpdatePost.ExpiredAt != default)
                {
                    request.UpdatePost.ExpiredAt = request.UpdatePost.ExpiredAt.ToUniversalTime();
                }

                postEntity.Title = request.UpdatePost.Title ?? postEntity.Title;
                postEntity.Description = request.UpdatePost.Description ?? postEntity.Description;
                postEntity.Quantity = request.UpdatePost.Quantity != default ? request.UpdatePost.Quantity : postEntity.Quantity;
                postEntity.Location = request.UpdatePost.Location ?? postEntity.Location;
                postEntity.ExpiredAt = request.UpdatePost.ExpiredAt != default ? request.UpdatePost.ExpiredAt : postEntity.ExpiredAt;
                postEntity.UnitMeasure = request.UpdatePost.UnitMeasure ?? postEntity.UnitMeasure;

                if (request.UpdatePost.Flower != null && postEntity.Flower != null)
                {
                    postEntity.Flower.Name = request.UpdatePost.Flower.Name ?? postEntity.Flower.Name;
                    postEntity.Flower.Price = request.UpdatePost.Flower.Price != default ? request.UpdatePost.Flower.Price : postEntity.Flower.Price;
                }

                // Lưu các thay đổi vào database
                //await _postRepository.UpdateAsync(postEntity);
                await _unitOfWork.SaveChangesAsync();

                return request.UpdatePost;
            }
            catch (NotFoundException)
            {
                throw new NotFoundException("Post or Flower not found");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the post: " + ex.Message);
            }
        }
    }
}
