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
using System.ComponentModel.DataAnnotations;

namespace Application.UserStore.Queries.GetStoreInDetails
{
    public record class GetStoreInDetailsByIdQuery : IRequest<StoreViewInDetailsDTO>
    {
        [Required]
        public Guid StoreID { get; init; }
    }

    public class GetStoreInDetailsByIdQueryHandler : IRequestHandler<GetStoreInDetailsByIdQuery, StoreViewInDetailsDTO>
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;

        public GetStoreInDetailsByIdQueryHandler(IServiceProvider serviceProvider)
        {
            _storeRepository = serviceProvider.GetRequiredService<IStoreRepository>();
            _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork<FlowerExchangeDbContext>>();
            _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
            _currentUser = serviceProvider.GetRequiredService<ICurrentUser>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();
        }
        public async Task<StoreViewInDetailsDTO> Handle(GetStoreInDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var store = await _storeRepository.FindAsyncWithIncludesAsync(store => store.Id == request.StoreID, store => store.Owner);
            if (store == null)
            {
                throw new NotFoundException(request.StoreID.ToString(), nameof(Store));
            }
            StoreViewInDetailsDTO storeViewInDetailsDTO = _mapper.Map<StoreViewInDetailsDTO>(store);
            return storeViewInDetailsDTO;
        }
    }
}
