using Application.UserWallet.DTOs;
using Application.UserWallet.Queries.GetAllWalletTransactionQuery;
using AutoMapper;
using Domain.Entities;
using Domain.Models;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using Transaction = System.Transactions.Transaction;

namespace Application.UserWallet.Queries.GetAllWalletTransactionQuery
{
    public class GetAllWalletTransactionQuery : IRequest<PagedList<WalletTransactionListResponse>>
    {
        public WalletTransactionParameter walletTransactionParameter { get; set; }
        public GetAllWalletTransactionQuery(WalletTransactionParameter _walletTransactionParameter = null)
        {
            walletTransactionParameter = _walletTransactionParameter;
        }
    }
}

public class GetAllWalletTransactionQueryHandler : IRequestHandler<GetAllWalletTransactionQuery, PagedList<WalletTransactionListResponse>>
{
    private IWalletTransactionRepository _iWalletTransactionRepository;

    private readonly ILogger<GetUserPostQueryHandler> _logger;

    private readonly IMapper _mapper;

    public GetAllWalletTransactionQueryHandler(IWalletTransactionRepository iWalletTransactionRepository, ILogger<GetUserPostQueryHandler> logger, IMapper mapper)
    {
        _iWalletTransactionRepository = iWalletTransactionRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<PagedList<WalletTransactionListResponse>> Handle(GetAllWalletTransactionQuery request, CancellationToken cancellationToken)
    {
        var walletTransactions = await _iWalletTransactionRepository.GetAllWalletTransactionAsync(request.walletTransactionParameter);

        //if (walletTransactions == null || !walletTransactions.Any())
        //{
        //    // Handle case when no posts are found for the user
        //    var errorMessage = $"No wallet transactions found !!!";
        //    _logger.LogWarning(errorMessage);
        //    throw new NotFoundException(errorMessage);
        //}
        var response = _mapper.Map<PagedList<WalletTransactionListResponse>>(walletTransactions);
        return new PagedList<WalletTransactionListResponse>(response, walletTransactions.TotalCount, walletTransactions.CurrentPage, walletTransactions.PageSize);
    }
}
