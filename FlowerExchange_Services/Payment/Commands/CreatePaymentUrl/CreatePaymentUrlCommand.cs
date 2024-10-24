using Application.Payment.DTOs;
using Domain.Entities;
using Domain.Payment;
using Domain.Payment.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Payment.Commands.CreatePaymentUrl
{
    public record CreatePaymentUrlCommand : IRequest<string>
    {
        public WalletDepositRequest WalletDepositRequest { get; init; } = default;

        public HttpContext HttpContext { get; set; }

        public string CurrentPath {  get; set; }

        public CreatePaymentUrlCommand(WalletDepositRequest walletDepositRequest, HttpContext httpContext, string currentPath)
        {
            WalletDepositRequest = walletDepositRequest;
            HttpContext = httpContext;
            CurrentPath = currentPath;
        }
    }

    public class CreatePaymentUrlCommandHandler : IRequestHandler<CreatePaymentUrlCommand , string>
    {
        private IConfiguration _configuration;
        private IVNPAYService _VNPAYService;
        private IMomoService _momoService;

        public CreatePaymentUrlCommandHandler(IConfiguration configuration, IVNPAYService vNPAYService, IMomoService momoService)
        {
            _configuration = configuration;
            _VNPAYService = vNPAYService;
            _momoService = momoService;
        }

        public async Task<string> Handle(CreatePaymentUrlCommand request, CancellationToken cancellationToken)
        {

            var userIdClaim = request.HttpContext.User.FindFirst(JwtRegisteredClaimNames.Jti);
            if (userIdClaim == null)
            {
                throw new Exception("Unauthorized");
            }
            var userId = userIdClaim.Value;
            var walletDeposit = new WalletDeposit
            {
                Amount = request.WalletDepositRequest.Amount,
                UserId = userId
            };

            // VNPAY
            var paymentUrl = _VNPAYService.CreatePaymentUrl(walletDeposit, request.HttpContext, request.CurrentPath);
            return paymentUrl;

            //Momo
            //var momoResponse = await _momoService.CreatePaymentAsync(walletDeposit, request.CurrentPath);

            //return momoResponse.PayUrl;
        }
    }
}
