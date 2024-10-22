using MediatR;
using Domain.Entities;
using Domain.Exceptions; // Nếu bạn có ngoại lệ NotFoundException
using System.Threading;
using System.Threading.Tasks;
using Application.SystemUser.DTOs;
using Domain.Repository;

namespace Application.SystemUser.Commands.UpdateUser
{
    public class UpdateUserAccountCommand : IRequest<string>
    {
        public UpdateUserAccountDTO UpdateUserAccountDTO { get; init; }

        public UpdateUserAccountCommand(UpdateUserAccountDTO updateUserAccountDTO)
        {
            UpdateUserAccountDTO = updateUserAccountDTO;
        }
    }

    public class UpdateUserAccountCommandHandler : IRequestHandler<UpdateUserAccountCommand, string>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserAccountCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> Handle(UpdateUserAccountCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UpdateUserAccountDTO.UserId);
            if (user == null) throw new NotFoundException("User not found");

            // Cập nhật thông tin người dùng
            if (!string.IsNullOrEmpty(request.UpdateUserAccountDTO.NewEmail))
            {
                user.Email = request.UpdateUserAccountDTO.NewEmail;
            }

            if (!string.IsNullOrEmpty(request.UpdateUserAccountDTO.NewUsername))
            {
                user.UserName = request.UpdateUserAccountDTO.NewUsername;
            }

            if (!string.IsNullOrEmpty(request.UpdateUserAccountDTO.Fullname))
            {
                user.Fullname = request.UpdateUserAccountDTO.Fullname;
            }

            if (!string.IsNullOrEmpty(request.UpdateUserAccountDTO.ProfilePictureUrl))
            {
                user.ProfilePictureUrl = request.UpdateUserAccountDTO.ProfilePictureUrl;
            }

            // Cập nhật thời gian cập nhật
            user.UpdatedAt = DateTimeOffset.UtcNow;

            await _userRepository.UpdateAsync(user);

            return "User account updated successfully";
        }
    }
}
