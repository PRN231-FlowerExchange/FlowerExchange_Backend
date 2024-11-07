
using Application.UserWallet.DTOs;
using Application.UserWallet.Queries.GetAllWalletTransactionQuery;
using Application.UserWallet.Queries.GetDetailWalletTransactionQuery;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalletTransactionController : APIControllerBase
    {
        private readonly ILogger<PostController> _logger;
        public WalletTransactionController(ILogger<PostController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public async Task<PagedList<WalletTransactionListResponse>> GetAllWalletTransaction([FromQuery] WalletTransactionParameter walletTransactionParameter)
        {
            return await Mediator.Send(new GetAllWalletTransactionQuery(walletTransactionParameter));
        }

        [HttpGet("{id}")]
        public async Task<WalletTransaction> GetDetailWalletTransaction(Guid id)
        {
            return await Mediator.Send(new GetDetailWalletTransactionQuery(id));
        }
    }
}
