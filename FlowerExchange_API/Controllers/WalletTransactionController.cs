
using System.IdentityModel.Tokens.Jwt;
using Application.UserWallet.DTOs;
using Application.UserWallet.Queries.GetAllWalletTransactionQuery;
using Application.UserWallet.Queries.GetDetailWalletTransactionQuery;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/wallet-transaction")]
    public class WalletTransactionController : APIControllerBase
    {
        private readonly ILogger<PostController> _logger;
        public WalletTransactionController(ILogger<PostController> logger)
        {
            _logger = logger;
        }
        
        // [HttpGet]
        // public async Task<PagedList<WalletTransactionListResponse>> GetAllWalletTransaction([FromQuery] WalletTransactionParameter walletTransactionParameter)
        // {
        //     return await Mediator.Send(new GetAllWalletTransactionQuery(walletTransactionParameter));
        // }

        [HttpGet("{id}/user/{userId}")]
        public async Task<IActionResult> GetDetailWalletTransaction(Guid id, Guid userId)
        {
            // Take userid from token
            // var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Jti);
            // if (userIdClaim == null)
            // {
            //     return Unauthorized();
            // }
            // var userId = Guid.Parse(userIdClaim.Value);
            
            var response = await Mediator.Send(new GetDetailWalletTransactionQuery(id, userId));
            return Ok(response);
        }
    }
}
