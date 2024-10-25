using Application.UserWallet.Queries.GetAllWalletTransactionQuery;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Models;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserWallet.Queries.GetAllWalletTransactionQuery
{
    public class GetAllWalletTransactionQuery : IRequest<PagedList<WalletTransaction>>
    {
        public WalletTransactionParameter walletTransactionParameter { get; set; }
        public GetAllWalletTransactionQuery(WalletTransactionParameter _walletTransactionParameter = null)
        {
            walletTransactionParameter = _walletTransactionParameter;
        }
    }
}

public class GetAllWalletTransactionQueryHandler : IRequestHandler<GetAllWalletTransactionQuery, PagedList<WalletTransaction>>
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

    public async Task<PagedList<WalletTransaction>> Handle(GetAllWalletTransactionQuery request, CancellationToken cancellationToken)
    {
        var walletTransactions = await _iWalletTransactionRepository.GetAllWalletTransactionAsync(request.walletTransactionParameter);

        //if (walletTransactions == null || !walletTransactions.Any())
        //{
        //    // Handle case when no posts are found for the user
        //    var errorMessage = $"No wallet transactions found !!!";
        //    _logger.LogWarning(errorMessage);
        //    throw new NotFoundException(errorMessage);
        //}
        var response = _mapper.Map<PagedList<WalletTransaction>>(walletTransactions);
        return new PagedList<WalletTransaction>(response, walletTransactions.TotalCount, walletTransactions.CurrentPage, walletTransactions.PageSize);
    }
}
