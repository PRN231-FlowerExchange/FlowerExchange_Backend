using Domain.Entities;
using Domain.Payment;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Constants.Enums;
using Domain.Repository;
using Application.Payment.DTOs;

namespace Application.Payment.Commands.CreateTransaction
{
    public record CreateTransactionCommand : IRequest<IPNResponseVNPAY>
    {
        public IQueryCollection vnpayResponseQueries { get; set; }

        public CreateTransactionCommand(IQueryCollection vnpayResponseQueries)
        {
            this.vnpayResponseQueries = vnpayResponseQueries;
        }
    }

    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, IPNResponseVNPAY>
    {
        private IVNPAYService _vnpayService;
        private IWalletRepository _walletRepository;
        private ITransactionRepository _transactionRepository;
        private IWalletTransactionRepository _walletTransactionRepository;

        public CreateTransactionCommandHandler(IVNPAYService vnpayService, IWalletRepository walletRepository, ITransactionRepository transactionRepository, IWalletTransactionRepository walletTransactionRepository)
        {
            _vnpayService = vnpayService;
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
            _walletTransactionRepository = walletTransactionRepository;
        }

        public async Task<IPNResponseVNPAY> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get vnpay payment response
                var vnpayPaymentResponse = _vnpayService.PaymentExecute(request.vnpayResponseQueries);

                if(vnpayPaymentResponse == null)
                {
                    return new IPNResponseVNPAY
                    {
                        RspCode = "01",
                        Message = "Payment status update unsuccess!"
                    };
                }

                // Find wallet id of user buy user id
                var userWallet = await _walletRepository.GetByUserId(vnpayPaymentResponse.UserId);
                
                // Create wallet for user if userWallet is null
                if (userWallet == null)
                {
                    userWallet = new Domain.Entities.Wallet
                    {
                        UserId = vnpayPaymentResponse.UserId,
                        TotalBalance = 0,
                        Currency = Currency.VND,
                    };

                    await _walletRepository.InsertAsync(userWallet);
                    await _walletRepository.SaveChagesAysnc();
                }

                // Create new transaction
                var transaction = new Transaction
                {
                    Amount = vnpayPaymentResponse.Amount,
                    Status = TransStatus.NotKnown,
                    Type = TransactionType.Deposit,
                    FromWallet = Guid.NewGuid(), // Can be null cause from system to user
                    ToWallet = userWallet.Id,
                    createById = vnpayPaymentResponse.UserId,
                    updateById = vnpayPaymentResponse.UserId,
                    //CreatedAt = vnpayPaymentResponse.PayDate,
                    //UpdatedAt = vnpayPaymentResponse.PayDate
                };

                await _transactionRepository.InsertAsync(transaction);
                await _transactionRepository.SaveChagesAysnc();

                // Create wallet transaction of wallet and transaction
                var plusTransaction = new WalletTransaction
                {
                    WalletId = userWallet.Id,
                    TransactonId = transaction.Id,
                    Type = TransDirection.Plus
                };
                await _walletTransactionRepository.InsertAsync(plusTransaction);
                await _walletTransactionRepository.SaveChagesAysnc();

                // Update wallet total balance 
                userWallet.TotalBalance += vnpayPaymentResponse.Amount;
                await _walletRepository.UpdateByIdAsync(userWallet, userWallet.Id);
                await _walletRepository.SaveChagesAysnc();

                return new IPNResponseVNPAY
                {
                    RspCode = "00",
                    Message = "Payment status update success."
                };

            }
            catch
            {
                throw;
            }
        }
    }
}
