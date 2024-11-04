using Application.UserIdentity.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repository;
using Domain.Security.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Extensions;

namespace Application.UserIdentity.Queries.CurrentUser
{
    public class CurrentUserQuery : IRequest<CurrentUserModel>
    {

    }

    public class CurentUserQueryHandler : IRequestHandler<CurrentUserQuery, CurrentUserModel>
    {
        private readonly ICurrentUser _curentUser;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IStoreRepository _storeRepository;
        private readonly IWalletRepository _walletRepository;

        public CurentUserQueryHandler(IServiceProvider serviceProvider)
        {
            _curentUser = serviceProvider.GetRequiredService<ICurrentUser>();
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();
            _storeRepository = serviceProvider.GetRequiredService<IStoreRepository>();
            _walletRepository = serviceProvider.GetRequiredService<IWalletRepository>();

        }

        async Task<CurrentUserModel> IRequestHandler<CurrentUserQuery, CurrentUserModel>.Handle(CurrentUserQuery request, CancellationToken cancellationToken)
        {
            if (_curentUser.IsAuthenticated)
            {
                string email = _curentUser.UserEmail;
                Console.WriteLine(email);
                User currentUserLogin = await _userManager.FindByEmailAsync(email);
                if (currentUserLogin == null)
                {
                    throw new NotFoundException("Current user is not found !");
                }

                CurrentUserModel currentUser = _mapper.Map<CurrentUserModel>(currentUserLogin);
                IList<string> roles = await _userManager.GetRolesAsync(currentUserLogin);
                currentUser.Roles = roles;
                currentUser.StatusDisplayName = currentUserLogin.Status.GetDisplayName();

                Store store = await _storeRepository.FirstOrDefaultAsync(x => x.OwnerId.Equals(currentUserLogin.Id));

                Domain.Entities.Wallet wallet = await _walletRepository.FirstOrDefaultAsync(x => x.UserId.Equals(currentUserLogin.Id));
                currentUser.StoreId = store == null ? null : store.Id;
                currentUser.WalletId = wallet == null ? null : wallet.Id;

                return currentUser;
            }
            else
            {
                throw new UnauthorizedAccessException("Access denied");
            }
        }
    }
}
