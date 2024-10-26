using Application.UserStore.DTOs;
using AutoMapper;
using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repository;
using Domain.Security.Identity;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Persistence;


namespace Application.UserStore.Command.CreateUserStore
{
    public record CreateUserStoreCommand : IRequest<Guid>
    {
        public StoreCreateDTO StoreCreateDTO { get; init; }
    }

    public class CreateUserStoreCommandHandler : IRequestHandler<CreateUserStoreCommand, Guid>
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;
        public CreateUserStoreCommandHandler(IServiceProvider serviceProvider)
        {
            _storeRepository = serviceProvider.GetRequiredService<IStoreRepository>();
            _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork<FlowerExchangeDbContext>>();
            _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
            _currentUser = serviceProvider.GetRequiredService<ICurrentUser>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();
        }
        public async Task<Guid> Handle(CreateUserStoreCommand request, CancellationToken cancellationToken)
        {
            Guid currentUserId = _currentUser.UserId;
            if (currentUserId == null)
            {
                throw new UnauthorizedAccessException();
            }

            User user = await _userRepository.GetByIdAsync(currentUserId);
            if (user == null)
            {
                throw new NotFoundException("Not found user to perform request");
            }
            if (user.Store != null)
            {
                throw new ConflictException("User already has a store and cannot create another one.");

            }

            StoreCreateDTO storeCreateDTO = request.StoreCreateDTO;
            Store store = _mapper.Map<Store>(storeCreateDTO);
            store.OwnerId = currentUserId;
            try
            {
                await _storeRepository.InsertAsync(store);
                await _unitOfWork.SaveChangesAsync();
                Store existingStore = await _storeRepository.FindAsync(s => s.Name.Equals(store.Name, StringComparison.OrdinalIgnoreCase));
                return existingStore.Id;
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackChanges();
                throw;
            }
        }
    }
}
