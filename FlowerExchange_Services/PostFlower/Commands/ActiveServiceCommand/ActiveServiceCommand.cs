using Application.PostFlower.DTOs;
using Application.PostFlower.Services;
using AutoMapper;
using Domain.Commons.BaseRepositories;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using Persistence;
using System.Collections.Generic;
using Entity = Domain.Entities;

namespace Application.PostFlower.Commands.ActiveServiceCommand
{
    public class ActiveServiceCommand : IRequest<PostViewDTO>
    {
        public Guid PostId { get; set; }
        public Guid ServiceId { get; set; }
        public int ServiceDay { get; set; } = 1;
    }

    public class AddServiceCommandHandle : IRequestHandler<ActiveServiceCommand, PostViewDTO>
    {
        private IMapper _mapper;
        private IServiceRepository _serviceRepository;
        private IPostRepository _postRepository;
        private IPostServiceRepository _postServiceRepository;
        private IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;

        public AddServiceCommandHandle(
            IMapper mapper,
            IServiceRepository serviceRepository,
            IPostRepository postRepository,
            IPostServiceRepository postServiceRepository,
            IUnitOfWork<FlowerExchangeDbContext> unitOfWork
            )
        {
            _mapper = mapper;
            _serviceRepository = serviceRepository;
            _postRepository = postRepository;
            _postServiceRepository = postServiceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PostViewDTO> Handle(ActiveServiceCommand request, CancellationToken cancellationToken)
        {
            // Fetch the post
            Entity.Post post = await _postRepository.GetByIdAsync(request.PostId);

            if (post == null)
            {
                throw new NotFoundException("Post not found");
            }

            Entity.PostService entity = await _postServiceRepository.GetAsync(request.PostId, request.ServiceId);

            if (entity == null) {
                throw new NotFoundException("PostService not found");
            }

            entity.ExpiredAt = DateTime.UtcNow.AddDays(request.ServiceDay);

            await _unitOfWork.SaveChangesAsync();
            return ConvertPostToView(post);
        }

        private PostViewDTO ConvertPostToView(Entity.Post source)
        {
            return ConvertFuction.ConvertObjectToObject<PostViewDTO, Entity.Post>(source);
        }
    }

}
