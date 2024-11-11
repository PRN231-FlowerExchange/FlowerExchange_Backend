using Application.UserWallet.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Models;
using Domain.Repository;
using MediatR;

namespace Application.UserWallet.Queries.GetWalletTransactionsOfUserWallet;

public record GetWalletTransactionsOfUserWalletQuery : IRequest<PagedList<WalletTransactionOfUserListResponse>>
{
    public Guid UserId { get; set; }
    
    public WalletTransactionParameter? WalletTransactionParameter { get; set; }

    public GetWalletTransactionsOfUserWalletQuery(Guid userId, WalletTransactionParameter? walletTransactionParameter)
    {
        UserId = userId;
        WalletTransactionParameter = walletTransactionParameter;
    }
}

public class GetWalletTransactionsOfUserWalletQueryHandler : IRequestHandler<GetWalletTransactionsOfUserWalletQuery, PagedList<WalletTransactionOfUserListResponse>>
{
    private IWalletTransactionRepository _walletTransactionRepository;
    
    private IWalletRepository _walletRepository;
    
    private IUserRepository _userRepository;

    private IMapper _mapper;

    public GetWalletTransactionsOfUserWalletQueryHandler(IWalletTransactionRepository walletTransactionRepository, IWalletRepository walletRepository, IUserRepository userRepository, IMapper mapper)
    {
        _walletTransactionRepository = walletTransactionRepository;
        _walletRepository = walletRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<PagedList<WalletTransactionOfUserListResponse>> Handle(GetWalletTransactionsOfUserWalletQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var wallet = await _walletRepository.GetByUserId(request.UserId);
            if (wallet == null)
            {
                throw new NotFoundException($"Wallet of user with id {request.UserId} was not found!");
            }

            var walletTransactions = await _walletTransactionRepository.GetWalletTransactionsByWalletIdAsync(wallet.Id, request.WalletTransactionParameter);
            
            var response = _mapper.Map<PagedList<WalletTransactionOfUserListResponse>>(walletTransactions);

            foreach (var transaction in response)
            {
                if (transaction.FromWallet != Guid.Empty)
                {
                    var fromUser = await _userRepository.GetUserByWalletId(transaction.FromWallet);
                    transaction.FromUserFullName = fromUser.Fullname;
                }
                
                if (transaction.ToWallet != Guid.Empty)
                {
                    var toUser = await _userRepository.GetUserByWalletId(transaction.ToWallet);
                    transaction.ToUserFullName = toUser.Fullname;
                }
            }
            
            return new PagedList<WalletTransactionOfUserListResponse>(response, walletTransactions.TotalCount, walletTransactions.CurrentPage, walletTransactions.PageSize);
        }
        catch
        {
            throw;
        }
    }
}