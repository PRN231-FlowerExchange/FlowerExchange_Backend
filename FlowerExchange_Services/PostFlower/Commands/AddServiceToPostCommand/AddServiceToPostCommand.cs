using Application.PostFlower.DTOs;
using Application.PostFlower.Services;
using AutoMapper;
using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Post post = await _postRepository.GetByIdAsync(request.Post.Id);

            if (post == null)
            {
                throw new NotFoundException("Post not found");
            }

            List<PostService> listPostService = new List<PostService>();
            foreach (ServiceViewDTO serviceDTO in request.ListService)
            {
                Service serviceEntity = await _serviceRepository.GetByIdAsync(serviceDTO.Id);

                if (serviceEntity == null) {
                    throw new NotFoundException("Service not found");
                }

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

                // Await asynchronous call
                await _postServiceRepository.InsertAsync(postService);
            }

            //post.PostServices = listPostService;

            return CovertPostToView(await _postRepository.GetByIdAsync(post.Id));
        }

        private PostViewDTO CovertPostToView(Post source)
        {
            return ConvertFuction.ConvertObjectToObject<PostViewDTO, Post>(source);
        }

    }
}
