﻿using CrossCuttingConcerns.Datetimes;
using Domain.Commons.BaseRepositories;
using Domain.Constants.Enums;
using Domain.Entities;
using Domain.Repository;
using MediatR;
using Persistence;

namespace Application.Payment.Commands.CreateFlowerServicePaymentTransaction;

public record CreateFlowerServicePaymentTransactionCommand : IRequest
{
    public Guid postId { get; init; }
    
    public Guid userId { get; init; }
    
    public CreateFlowerServicePaymentTransactionCommand(Guid postId, Guid userId)
    {
        this.postId = postId;
        this.userId = userId;
    }
}

public class
    CreateFlowerServicePaymentTransactionCommandHandler : IRequestHandler<CreateFlowerServicePaymentTransactionCommand>
{
    private IUserRepository _userRepository;
    private IPostRepository _postRepository;
    private IPostServiceRepository _postServiceRepository;
    private IWalletRepository _walletRepository;
    private IServiceOrderRepository _serviceOrderRepository;
    private ITransactionRepository _transactionRepository;
    private IWalletTransactionRepository _walletTransactionRepository;
    private readonly IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;
    private IFlowerRepository _flowerRepository;
    private IFlowerOrderRepository _flowerOrderRepository;
    private IDateTimeProvider _dateTimeProvider;

    public CreateFlowerServicePaymentTransactionCommandHandler(
        IUserRepository userRepository, 
        IPostRepository postRepository, 
        IPostServiceRepository postServiceRepository, 
        IWalletRepository walletRepository, 
        IServiceOrderRepository serviceOrderRepository, 
        ITransactionRepository transactionRepository, 
        IWalletTransactionRepository walletTransactionRepository, 
        IUnitOfWork<FlowerExchangeDbContext> unitOfWork,
        IFlowerRepository flowerRepository,
        IFlowerOrderRepository flowerOrderRepository,
        IDateTimeProvider dateTimeProvider
        )
    {
        _userRepository = userRepository;
        _postRepository = postRepository;
        _postServiceRepository = postServiceRepository;
        _walletRepository = walletRepository;
        _serviceOrderRepository = serviceOrderRepository;
        _transactionRepository = transactionRepository;
        _walletTransactionRepository = walletTransactionRepository;
        _unitOfWork = unitOfWork;
        _flowerRepository = flowerRepository;
        _flowerOrderRepository = flowerOrderRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Handle(CreateFlowerServicePaymentTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Find user by id
            var buyer = await _userRepository.GetByIdAsync(request.userId);
            if (buyer == null)
            {
                throw new Exception($"User with id {request.userId} not found in CreateFlowerServicePaymentTransactionCommand!");
            }
            
            // Find post include flower
            var post = await _postRepository.GetPostByIdWithFlowerAsync(request.postId);
            if (post == null)
            {
                throw new Exception($"Post with id {request.postId} not found in " +
                                    $"CreateFlowerServicePaymentTransactionCommand!");
            }

            if (post.PostStatus != PostStatus.Available)
            {
                throw new Exception($"Post with id {request.postId} is not available!");
            }

            double totalAmount = 0;

            // Get flower price
            totalAmount += post.Flower.Price * (double)post.Quantity;
            
            // Find wallet of buyer and check if balance is valid
            var buyerWallet = await _walletRepository.GetByUserId(buyer.Id);
            if (buyerWallet == null || buyerWallet.TotalBalance < totalAmount) // Check wallet balance
            {
                throw new Exception($"Wallet balance of user with id {buyer.Id} is invalid in " +
                                    $"CreateFlowerServicePaymentTransactionCommand!");
            }
            
            // Find wallet of seller
            var sellerWallet = await _walletRepository.GetByUserId((Guid)post.SellerId);
            if (sellerWallet == null) // Check wallet balance
            {
                throw new Exception($"Wallet of user with id {post.SellerId} not found in " +
                                    $"CreateFlowerServicePaymentTransactionCommand!");
            }
            
            // Create flower order
            var flowerOrder = new FlowerOrder
            {
                Amount = totalAmount,
                IsRefund = false,
                Status = OrderStatus.Success,
                BuyerId = buyer.Id,
                SellerId = (Guid)post.SellerId,
                FlowerId = post.Flower.Id,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            await _flowerOrderRepository.InsertAsync(flowerOrder);
            await _unitOfWork.SaveChangesAsync();

            // Create transaction
            var transaction = new Transaction
            {
                Amount = totalAmount,
                Status = Domain.Constants.Enums.TransStatus.Success,
                Type = Domain.Constants.Enums.TransactionType.Trade,
                FromWallet = buyerWallet.Id,
                ToWallet = sellerWallet.Id,
                FlowerOrderId = flowerOrder.Id,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            await _transactionRepository.InsertAsync(transaction);
            
            // Create wallet transaction
            // Plus direction
            var plusWalletTransaction = new WalletTransaction
            {
                WalletId = sellerWallet.Id,
                TransactonId = transaction.Id,
                Type = Domain.Constants.Enums.TransDirection.Plus,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            await _walletTransactionRepository.InsertAsync(plusWalletTransaction);
            
            // Minus direction
            var minusWalletTransaction = new WalletTransaction
            {
                WalletId = buyerWallet.Id,
                TransactonId = transaction.Id,
                Type = Domain.Constants.Enums.TransDirection.Minus,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            await _walletTransactionRepository.InsertAsync(minusWalletTransaction);
            
            // Update buyer and seller wallet balance
            // Buyer wallet
            buyerWallet.TotalBalance -= totalAmount;
            buyerWallet.UpdatedAt = DateTimeOffset.UtcNow;
            await _walletRepository.UpdateByIdAsync(buyerWallet, buyerWallet.Id);
            
            // Seller wallet
            sellerWallet.TotalBalance += totalAmount;
            sellerWallet.UpdatedAt = DateTimeOffset.UtcNow;
            await _walletRepository.UpdateByIdAsync(sellerWallet, sellerWallet.Id);
            
            // Update post status to "Sold out"
            post.PostStatus = PostStatus.SoldOut;
            post.UpdatedAt = DateTimeOffset.UtcNow;
            await _postRepository.UpdateByIdAsync(post, post.Id);

            await _unitOfWork.SaveChangesAsync();

        }
        catch
        {
            throw;
        }
}
}

