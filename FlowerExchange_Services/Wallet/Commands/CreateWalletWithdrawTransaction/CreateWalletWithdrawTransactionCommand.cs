using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Transactions;
using Domain.Commons.BaseRepositories;
using Domain.Constants.Enums;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using Persistence;
using Transaction = Domain.Entities.Transaction;

namespace Application.Wallet.Commands.CreateWalletWithdrawTransaction;

public record CreateWalletWithdrawTransactionCommand : IRequest
{
    [Required]
    [Range(50000, double.MaxValue)]
    public double Amount { get; init; }
    
    [JsonIgnore]
    public Guid? UserId { get; set; }

    public CreateWalletWithdrawTransactionCommand(double amount, Guid? userId)
    {
        Amount = amount;
        UserId = userId;
    }
}

public class CreateWalletWithdrawTransactionCommandHandler : IRequestHandler<CreateWalletWithdrawTransactionCommand>
{
    private IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;
    private IUserRepository _userRepository;
    private IWalletRepository _walletRepository;
    private ITransactionRepository _transactionRepository;
    private IWalletTransactionRepository _walletTransactionRepository;

    public CreateWalletWithdrawTransactionCommandHandler(IUnitOfWork<FlowerExchangeDbContext> unitOfWork, IUserRepository userRepository, IWalletRepository walletRepository, ITransactionRepository transactionRepository, IWalletTransactionRepository walletTransactionRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
        _walletTransactionRepository = walletTransactionRepository;
    }

    public async Task Handle(CreateWalletWithdrawTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.UserId == null)
            {
                throw new BadRequestException("User id id required!");
            }
            
            // Find user by user id
            var user = await _userRepository.GetByIdAsync((Guid)request.UserId);
            if (user == null)
            {
                throw new NotFoundException(
                    $"User with id {request.UserId} not found in CreateWalletWithdrawTransactionCommand!");
            }
            
            // Find wallet of user
            var wallet = await _walletRepository.GetByUserId(user.Id);
            if (wallet == null)
            {
                throw new NotFoundException(
                    $"Wallet of user with id {request.UserId} not found in CreateWalletWithdrawTransactionCommand!");
            }
            
            // Check total balance of wallet
            if (request.Amount > wallet.TotalBalance)
            {
                throw new BadRequestException("The amount is greater than the wallet total balance!");
            }
            
            // Create new transaction
            var transaction = new Domain.Entities.Transaction
            {
                Amount = request.Amount,
                Status = TransStatus.Success,
                Type = TransactionType.Withdraw,
                FromWallet = wallet.Id,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            await _transactionRepository.InsertAsync(transaction);
            
            // Create new wallet transaction
            var walletTransaction = new WalletTransaction
            {
                WalletId = wallet.Id,
                TransactonId = transaction.Id,
                Type = TransDirection.Minus,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            await _walletTransactionRepository.InsertAsync(walletTransaction);
            
            // Update user wallet total balance
            wallet.TotalBalance -= request.Amount;
            await _walletRepository.UpdateByIdAsync(wallet, wallet.Id);

            await _unitOfWork.SaveChangesAsync();
        }
        catch
        {
            throw;
        }
    }
}