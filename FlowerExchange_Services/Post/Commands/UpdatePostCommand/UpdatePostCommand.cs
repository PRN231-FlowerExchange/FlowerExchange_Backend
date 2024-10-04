using Application.Post.DTOs;
using Application.Post.Services;
using AutoMapper;
using Domain.Commons.BaseRepositories;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Post.Commands.UpdatePostCommand
{
    public record UpdatePostCommand : IRequest<PostUpdateDTO>
    {
        public PostUpdateDTO UpdatePost { get; init; } = default;
    }

    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, PostUpdateDTO>
    {
        private IMapper _mapper;
        private IPostRepository _postRepository;
        private IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;

        public UpdatePostCommandHandler(IMapper mapper, IPostRepository postRepository, IUnitOfWork<FlowerExchangeDbContext> unitOfWork)
        {
            _mapper = mapper;
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
        }

        //public async Task<Domain.Entities.Post> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        //{
        //    Domain.Entities.Post postEntity = _mapper.Map<Domain.Entities.Post>(request.UpdatePost);
        //    await _postRepository.UpdateAsync(postEntity);
        //    //Console.WriteLine($"Entity State Before Save: {_unitOfWork.Context.Entry(entity).State}");
        //    await _unitOfWork.SaveChangesAsync();
        //    return postEntity;
        //}

        public async Task<PostUpdateDTO> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {

            try
            {
                Domain.Entities.Post postEntity = await _postRepository.GetByIdAsync(request.UpdatePost.Id);
                //checking
                if (postEntity == null)
                {
                    throw new NotFoundException("Post not found");
                }
                //change information
                postEntity = CovertUpdatePostDTOToPost(request.UpdatePost);
                //update save async
                await _postRepository.UpdateAsync(postEntity);
                await _unitOfWork.SaveChangesAsync();


            }
            catch (NotFoundException)
            {
                throw new NotFoundException("Post not existed!");
            }
            //Domain.Entities.Post postEntity = _mapper.Map<Domain.Entities.Post>(request.UpdatePost);
            //await _postRepository.UpdateAsync(postEntity);
            //Console.WriteLine($"Entity State Before Save: {_unitOfWork.Context.Entry(entity).State}");
            await _unitOfWork.SaveChangesAsync();
            return request.UpdatePost;
        }

        private Domain.Entities.Post CovertUpdatePostDTOToPost(PostUpdateDTO source)
        {
            return ConvertFuction.ConvertObjectToObject<Domain.Entities.Post, PostUpdateDTO>(source);
        }
    }
}
