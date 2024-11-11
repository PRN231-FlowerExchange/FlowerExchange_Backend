using Application.Order.DTOs;
using AutoMapper;
using Domain.Constants.Enums;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;

namespace Application.Order.Queries;

public record GetHistoryFlowerOrderOfUserQuery : IRequest<List<FlowerOrderHistoryListResponse>>
{
    public Guid UserId { get; set; }

    public GetHistoryFlowerOrderOfUserQuery(Guid userId)
    {
        UserId = userId;
    }
}

public class GetHistoryFlowerOrderOfUserQueryHandler : IRequestHandler<GetHistoryFlowerOrderOfUserQuery, List<FlowerOrderHistoryListResponse>>
{
    private IWalletRepository _walletRepository;
    
    private IFlowerOrderRepository _flowerOrderRepository;
    
    private IMapper _mapper;    
    
    private IUserRepository _userRepository;

    public GetHistoryFlowerOrderOfUserQueryHandler(IWalletRepository walletRepository, IFlowerOrderRepository flowerOrderRepository, IMapper mapper, IUserRepository userRepository)
    {
        _walletRepository = walletRepository;
        _flowerOrderRepository = flowerOrderRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<List<FlowerOrderHistoryListResponse>> Handle(GetHistoryFlowerOrderOfUserQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new NotFoundException($"User with id {request.UserId} was not found!");
            }
            
            var wallet = await _walletRepository.GetByUserId(request.UserId);
            if (wallet == null)
            {
                throw new NotFoundException($"Wallet of user with id {request.UserId} was not found!");
            }

            var flowerOrders =
                await _flowerOrderRepository.GetFlowerOrdersByWalletIdAndTypeAndStatus(
                    wallet.Id,
                    TransactionType.Trade,
                    TransStatus.Success
                );

            var response = _mapper.Map<List<FlowerOrderHistoryListResponse>>(flowerOrders);

            foreach (var flowerOrder in response)
            {
                var seller = await _userRepository.GetByIdAsync(flowerOrder.SellerId);
                flowerOrder.SellerFullName = seller.Fullname;

                flowerOrder.BuyerFullName = user.Fullname;
            }

            return response;
        }
        catch
        {
            throw;
        }
    }
}