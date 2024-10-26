using Domain.Entities;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UserWallet.Queries.GetDetailWalletTransactionQuery
{
    public class GetDetailWalletTransactionQuery : IRequest<WalletTransaction>
    {
        public Guid Id { get; set; }
        public GetDetailWalletTransactionQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetDetailWalletTransactionQueryHandler : IRequestHandler<GetDetailWalletTransactionQuery, WalletTransaction>
    {
        private IWalletTransactionRepository _iWalletTransactionRepository;

        private readonly ILogger<GetDetailWalletTransactionQueryHandler> _logger;

        public GetDetailWalletTransactionQueryHandler(IWalletTransactionRepository iWalletTransactionRepository, ILogger<GetDetailWalletTransactionQueryHandler> logger)
        {
            _iWalletTransactionRepository = iWalletTransactionRepository;
            _logger = logger;
        }

        public async Task<WalletTransaction> Handle(GetDetailWalletTransactionQuery request, CancellationToken cancellationToken)
        {
            var walletTransaction = await _iWalletTransactionRepository.GetByIdAsync(request.Id);
            if (walletTransaction == null)
            {
                var errorMessage = $"Wallet Transaction with Id: {request.Id} was not found.";
                _logger.LogWarning(errorMessage);
                throw new NotFoundException(errorMessage);
            }
            return walletTransaction;
        }
    }
}


