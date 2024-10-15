using Application.Post.Queries.GetDetailPost;
using Application.Wallet.Queries;
using Application.Wallet.Queries.GetAllWalletTransactionQuery;
using Application.Wallet.Queries.GetDetailWalletTransactionQuery;
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
        public async Task<PagedList<WalletTransaction>> GetAllWalletTransaction([FromQuery] WalletTransactionParameter walletTransactionParameter)
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
