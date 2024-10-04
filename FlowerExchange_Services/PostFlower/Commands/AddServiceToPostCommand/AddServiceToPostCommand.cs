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
    public class AddServiceToPostCommand : IRequest<Post>
    {
        public Post Post { get; set; }
        public List<Service> ListService { get; set; }
        public int ServiceDay { get; set; }
    }

    public class AddServicePostCommandHandle : IRequestHandler<AddServiceToPostCommand, Post>
    {
        private IServiceRepository _serviceRepository;
        private IPostRepository _postRepository;
        private IPostServiceRepository _postServiceRepository;
        private IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;

        public AddServicePostCommandHandle(
            IServiceRepository serviceRepository,
            IPostRepository postRepository,
            IPostServiceRepository postServiceRepository,
            IUnitOfWork<FlowerExchangeDbContext> unitOfWork)
        {
            _serviceRepository = serviceRepository;
            _postRepository = postRepository;
            _postServiceRepository = postServiceRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<Post> Handle(AddServiceToPostCommand request, CancellationToken cancellationToken)
        {
            Post post = _postRepository.GetByIdAsync(request.Post.Id).GetAwaiter().GetResult();
            try
            {

                if (post == null)
                {
                    throw new NotFoundException("Post not found");
                }

                List<PostService> listPostService = new();
                foreach (var service in request.ListService)
                {
                    PostService postService = new()
                    {
                        PostId = request.Post.Id,
                        Post = request.Post,
                        ServiceId = service.Id,
                        Service = service,
                        CreatedAt = DateTime.UtcNow,
                        ExpiredAt = DateTime.UtcNow.AddDays(request.ServiceDay),
                    };
                    listPostService.Add(postService);
                    _postServiceRepository.InsertAsync(postService);
                }
                post.PostServices = listPostService;
            }
            catch (NotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
            }

            return _postRepository.GetByIdAsync(request.Post.Id);

        }
    }
}
