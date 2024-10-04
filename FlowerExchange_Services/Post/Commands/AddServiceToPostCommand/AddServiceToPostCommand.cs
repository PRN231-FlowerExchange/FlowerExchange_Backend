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

namespace Application.Post.Commands.AddServiceToPostCommand
{
    public class AddServiceToPostCommand : IRequest<Domain.Entities.Post>
    {
        public Domain.Entities.Post Post { get; set; }
        public List<Service> ListService { get; set; }
        public int ServiceDay { get; set; }
    }

    public class AddServicePostCommandHandle : IRequestHandler<AddServiceToPostCommand, Domain.Entities.Post>
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
            this._serviceRepository = serviceRepository;
            this._postRepository = postRepository;
            this._postServiceRepository = postServiceRepository;
            this._unitOfWork = unitOfWork;
        }

        public Task<Domain.Entities.Post> Handle(AddServiceToPostCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.Post post = _postRepository.GetByIdAsync(request.Post.Id).GetAwaiter().GetResult();
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
            catch (Exception ex) {
            }

            return _postRepository.GetByIdAsync(request.Post.Id);

        }
    }
}
