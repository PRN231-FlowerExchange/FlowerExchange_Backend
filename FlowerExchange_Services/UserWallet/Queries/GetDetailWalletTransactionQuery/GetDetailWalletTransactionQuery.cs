using Application.UserWallet.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UserWallet.Queries.GetDetailWalletTransactionQuery
{
    public class GetDetailWalletTransactionQuery : IRequest<WalletTransactionOfUserDetailsResponse?>
    {
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }

        public GetDetailWalletTransactionQuery(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
    }

    public class GetDetailWalletTransactionQueryHandler : IRequestHandler<GetDetailWalletTransactionQuery, WalletTransactionOfUserDetailsResponse?>
    {
        private IWalletTransactionRepository _iWalletTransactionRepository;

        private readonly ILogger<GetDetailWalletTransactionQueryHandler> _logger;
        
        private IMapper _mapper;
        
        private IUserRepository _userRepository;

        public GetDetailWalletTransactionQueryHandler(IWalletTransactionRepository iWalletTransactionRepository, ILogger<GetDetailWalletTransactionQueryHandler> logger, IMapper mapper, IUserRepository userRepository)
        {
            _iWalletTransactionRepository = iWalletTransactionRepository;
            _logger = logger;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<WalletTransactionOfUserDetailsResponse?> Handle(GetDetailWalletTransactionQuery request, CancellationToken cancellationToken)
        {
            var walletTransaction = await _iWalletTransactionRepository.GetWalletTransactionByTransactionIdAsync(request.Id, request.UserId);
            
            if (walletTransaction == null)
            {
                var errorMessage = $"Wallet Transaction with Id: {request.Id} was not found or it not belong to user with id {request.UserId}.";
                _logger.LogWarning(errorMessage);
                throw new NotFoundException(errorMessage);
            }

            var response = _mapper.Map<WalletTransactionOfUserDetailsResponse>(walletTransaction);
            
            if (response.FromWallet != Guid.Empty)
            {
                var fromUser = await _userRepository.GetUserByWalletId(response.FromWallet);
                response.FromUserFullName = fromUser.Fullname;
            }
                
            if (response.ToWallet != Guid.Empty)
            {
                var toUser = await _userRepository.GetUserByWalletId(response.ToWallet);
                response.ToUserFullName = toUser.Fullname;
            }
            
            return response;
        }
    }
}


