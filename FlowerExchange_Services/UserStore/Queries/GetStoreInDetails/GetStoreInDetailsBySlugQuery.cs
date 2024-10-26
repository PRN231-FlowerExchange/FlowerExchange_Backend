using Application.UserStore.DTOs;
using AutoMapper;
using Domain.Commons.BaseRepositories;
using Domain.Exceptions;
using Domain.Repository;
using Domain.Security.Identity;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using System.ComponentModel.DataAnnotations;

namespace Application.UserStore.Queries.GetStoreInDetails
{
    public record class GetStoreInDetailsBySlugQuery : IRequest<StoreViewInDetailsDTO>
    {
        [Required]
        public string Slug { get; init; }
    }

    public class GetStoreInDetailsBySlugQueryHandler : IRequestHandler<GetStoreInDetailsBySlugQuery, StoreViewInDetailsDTO>
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;

        public GetStoreInDetailsBySlugQueryHandler(IServiceProvider serviceProvider)
        {
            _storeRepository = serviceProvider.GetRequiredService<IStoreRepository>();
            _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork<FlowerExchangeDbContext>>();
            _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
            _currentUser = serviceProvider.GetRequiredService<ICurrentUser>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();
        }
        public async Task<StoreViewInDetailsDTO> Handle(GetStoreInDetailsBySlugQuery request, CancellationToken cancellationToken)
        {
            var store = await _storeRepository.FindAsyncWithIncludesAsync(store => store.Slug.Equals(request.Slug), store => store.Owner);
            if (store == null)
            {
                throw new NotFoundException();
            }
            StoreViewInDetailsDTO storeViewInDetailsDTO = _mapper.Map<StoreViewInDetailsDTO>(store);
            return storeViewInDetailsDTO;
        }
    }
}
