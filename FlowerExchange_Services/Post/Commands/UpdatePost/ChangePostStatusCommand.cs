using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Commons.BaseRepositories;
using Domain.Constants.Enums;
using Domain.Repository;
using MediatR;
using Persistence;

namespace Application.Post.Commands.ChangePostStatus
{
    // Định nghĩa Command để thay đổi trạng thái bài đăng
    public class ChangePostStatusCommand : IRequest<bool>
    {
        public Guid PostId { get; init; }
        public PostStatus NewStatus { get; init; }

        public ChangePostStatusCommand(Guid postId, PostStatus newStatus)
        {
            PostId = postId;
            NewStatus = newStatus;
        }
    }

    // Định nghĩa Handler để xử lý thay đổi trạng thái
    public class ChangePostStatusCommandHandler : IRequestHandler<ChangePostStatusCommand, bool>
    {
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;

        public ChangePostStatusCommandHandler(
            IPostRepository postRepository,
            IUnitOfWork<FlowerExchangeDbContext> unitOfWork)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ChangePostStatusCommand request, CancellationToken cancellationToken)
        {
            // Lấy post từ database dựa trên PostId
            var post = await _postRepository.GetByIdAsync(request.PostId);
            if (post == null)
            {
                throw new Exception("Post not found");
            }

            // Cập nhật trạng thái bài đăng
            post.PostStatus = request.NewStatus;

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
