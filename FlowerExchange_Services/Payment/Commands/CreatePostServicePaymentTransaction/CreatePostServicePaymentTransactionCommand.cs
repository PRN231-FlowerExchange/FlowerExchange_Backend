using Application.Payment.DTOs;
using Domain.Entities;
using Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Commons.BaseRepositories;
using Persistence;

namespace Application.Payment.Commands.CreatePostServicePaymentTransaction
{
    public record CreatePostServicePaymentTransactionCommand : IRequest
    {
        public PostServicePaymentRequest postServicePaymentRequest { get; set; }

        public Guid userId { get; set; }

        public CreatePostServicePaymentTransactionCommand(PostServicePaymentRequest postServicePaymentRequest, Guid userId)
        {
            this.postServicePaymentRequest = postServicePaymentRequest;
            this.userId = userId;
        }

    }

    public class CreatePostServicePaymentTransactionCommandHandler : IRequestHandler<CreatePostServicePaymentTransactionCommand>
    {
        private IUserRepository _userRepository;
        private IPostRepository _postRepository;
        private IPostServiceRepository _postServiceRepository;
        private IWalletRepository _walletRepository;
        private IServiceOrderRepository _serviceOrderRepository;
        private ITransactionRepository _transactionRepository;
        private IWalletTransactionRepository _walletTransactionRepository;
        private readonly IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;

        public CreatePostServicePaymentTransactionCommandHandler(IUserRepository userRepository, IPostRepository postRepository, IPostServiceRepository postServiceRepository, IWalletRepository walletRepository, IServiceOrderRepository serviceOrderRepository, ITransactionRepository transactionRepository, IWalletTransactionRepository walletTransactionRepository, IUnitOfWork<FlowerExchangeDbContext> unitOfWork)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _postServiceRepository = postServiceRepository;
            _walletRepository = walletRepository;
            _serviceOrderRepository = serviceOrderRepository;
            _transactionRepository = transactionRepository;
            _walletTransactionRepository = walletTransactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CreatePostServicePaymentTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Find user by id
                var user = await _userRepository.GetByIdAsync(request.userId);
                if (user == null)
                {
                    throw new Exception($"User with id {request.userId} not found in CreatePostServicePaymentTransactionCommand!");
                }

                var postIds = request.postServicePaymentRequest.PostIds;
                if (postIds == null || postIds.Length <= 0)
                {
                    throw new Exception($"PostIds is required in CreatePostServicePaymentTransactionCommand!");
                }

                var inputServiceIds = request.postServicePaymentRequest.ServiceIds;
                double totalAmount = 0;
                var selectedPostServices = new List<PostService>();

                foreach (Guid postId in postIds)
                {
                    await GetPostServicesForSelectedPostServicesAndTotalAmount(
                            postId,
                            selectedPostServices,
                            totalAmount,
                            inputServiceIds.ToList()
                        );

                }

                // Find user wallet 
                var wallet = await _walletRepository.GetByUserId(user.Id);
                if (wallet == null || wallet.TotalBalance < totalAmount) // Check wallet balance
                {
                    throw new Exception($"Wallet balance of user with id {user.Id} is invalid in CreatePostServicePaymentTransactionCommand!");
                }

                // Create service order
                var serviceOrder = new ServiceOrder
                {
                    Amount = totalAmount,
                    Status = Domain.Constants.Enums.OrderStatus.Success,
                    BuyerId = user.Id,

                };
                await _serviceOrderRepository.InsertAsync(serviceOrder);
                // await _serviceOrderRepository.SaveChagesAysnc();

                // Update serviceOrderId for each postService
                foreach (PostService postService in selectedPostServices)
                {
                    postService.ServiceOrderId = serviceOrder.Id;
                    await _postServiceRepository.UpdateByIdAsync(postService, postService.Id);
                }
                // await _postServiceRepository.SaveChagesAysnc();

                // Create transaction
                var transaction = new Transaction
                {
                    Amount = totalAmount,
                    Status = Domain.Constants.Enums.TransStatus.NotKnown,
                    Type = Domain.Constants.Enums.TransactionType.Sell,
                    FromWallet = wallet.Id,
                    ToWallet = Guid.NewGuid(),
                    ServiceOrderId = serviceOrder.Id
                };
                await _transactionRepository.InsertAsync(transaction);
                // await _transactionRepository.SaveChagesAysnc();

                // Create wallet transaction
                var walletTransaction = new WalletTransaction
                {
                    WalletId = wallet.Id,
                    TransactonId = transaction.Id,
                    Type = Domain.Constants.Enums.TransDirection.Minus
                };
                await _walletTransactionRepository.InsertAsync(walletTransaction);
                // await _walletTransactionRepository.SaveChagesAysnc();

                // Update wallet balance
                wallet.TotalBalance -= totalAmount;
                await _walletRepository.UpdateByIdAsync(wallet, wallet.Id);
                // await _walletRepository.SaveChagesAysnc();

                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        private async Task GetPostServicesForSelectedPostServicesAndTotalAmount(Guid postId, List<PostService> selectedPostServices, double totalAmount, List<Guid> inputServiceIds)
        {
            // Find post by id
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
            {
                throw new Exception($"Post with id {postId} not found in CreatePostServicePaymentTransactionCommand!");
            }

            // Find post service by post id with status inactive and in serviceIds input
            // and add to total post services list
            var postServices = await _postServiceRepository.GetPostServicesByPostIdAndServiceIdsAndStatus(
                        postId,
                        inputServiceIds,
                        Domain.Constants.Enums.PostServiceStatus.Inactive
                    );
            if (postServices.Count() <= 0)
            {
                return;
            }
            selectedPostServices.AddRange(postServices);

            // Get price list of services then add to total amount
            var postServicePrices = postServices.Select(ps => ps.Service.Price).ToList();
            totalAmount += postServicePrices.Sum();
        }
    }
}
