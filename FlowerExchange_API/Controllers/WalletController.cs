using System.IdentityModel.Tokens.Jwt;
using Application.UserWallet.Queries.GetWalletTransactionsOfUserWallet;
using Application.Wallet.Commands.CreateWalletWithdrawTransaction;
using Application.Wallet.Queries.GetWalletDetailsOfUser;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/wallet")]
public class WalletController : APIControllerBase
{
    private readonly ILogger<WalletController> _logger;

    public WalletController(ILogger<WalletController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{userId}")]
    [Authorize]
    public async Task<IActionResult> GetWalletOfUser(Guid userId)
    {
        if (userId.Equals(Guid.Empty))
        {
            return BadRequest("User id is required!");
        }
        try
        {
            // Take userid from token
            // var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Jti);
            // if (userIdClaim == null)
            // {
            //     return Unauthorized();
            // }
            // var userId = Guid.Parse(userIdClaim.Value);
            
            var response = await Mediator.Send(new GetaWalletDetailsOfUserCommand(userId));
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message); 
        }
    }

    [HttpPost("withdraw")]
    [Authorize]
    public async Task<IActionResult> CreateWithdrawTransaction([FromBody] CreateWalletWithdrawTransactionCommand command)
    {
        // Take userid from token
        var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Jti);
        if (userIdClaim == null)
        {
            return Unauthorized();
        }
        var userId = Guid.Parse(userIdClaim.Value);
        command.UserId = userId;

        try
        {
            await Mediator.Send(command);

            return Ok(new { message = "Withdraw transaction success!" });
        }
        catch (Exception e) when (e is not NotFoundException && e is not BadRequestException)
        {
            return StatusCode(500, e.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{userId}/wallet-transaction")]
    public async Task<IActionResult> GetWalletTransactionsOfUserWallet(Guid userId, [FromQuery] WalletTransactionParameter walletTransactionParameter)
    {
        try
        {
            var response =
                await Mediator.Send(new GetWalletTransactionsOfUserWalletQuery(userId, walletTransactionParameter));
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}