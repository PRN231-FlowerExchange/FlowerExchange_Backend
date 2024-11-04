using Application.UserIdentity.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repository;
using Domain.Security.Identity;
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

namespace Application.UserIdentity.Queries.GetUser
{
    public class GetUserByIDPublicQuery : IRequest<CurrentUserModel>
    {
        public Guid UserID { get; init; }
    }

    public class GetUserByIDPublicQueryHandler : IRequestHandler<GetUserByIDPublicQuery, CurrentUserModel>
    {
        private readonly ICurrentUser _curentUser;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IStoreRepository _storeRepository;
        private readonly IWalletRepository _walletRepository;

        public GetUserByIDPublicQueryHandler(IServiceProvider serviceProvider)
        {
            _curentUser = serviceProvider.GetRequiredService<ICurrentUser>();
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();
            _storeRepository = serviceProvider.GetRequiredService<IStoreRepository>();
            _walletRepository = serviceProvider.GetRequiredService<IWalletRepository>();

        }

        async Task<CurrentUserModel> IRequestHandler<GetUserByIDPublicQuery, CurrentUserModel>.Handle(GetUserByIDPublicQuery request, CancellationToken cancellationToken)
        {
            Guid userId = request.UserID;
            User currentUserLogin = await _userManager.FindByIdAsync(userId.ToString());
            if (currentUserLogin == null)
            {
                throw new NotFoundException("User is not found !");
            }

            CurrentUserModel currentUser = _mapper.Map<CurrentUserModel>(currentUserLogin);
            IList<string> roles = await _userManager.GetRolesAsync(currentUserLogin);
            currentUser.Roles = roles;
            currentUser.StatusDisplayName = currentUserLogin.Status.GetDisplayName();

            Store store = await _storeRepository.FirstOrDefaultAsync(x => x.OwnerId.Equals(currentUserLogin.Id));

            Wallet wallet = await _walletRepository.FirstOrDefaultAsync(x => x.UserId.Equals(currentUserLogin.Id));
            currentUser.StoreId = store == null ? null : store.Id;
            currentUser.WalletId = wallet == null ? null : wallet.Id;

            return currentUser;
        }
    }
}
