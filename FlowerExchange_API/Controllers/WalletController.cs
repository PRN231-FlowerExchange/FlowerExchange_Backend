using System.IdentityModel.Tokens.Jwt;
using Application.Wallet.Queries.GetWalletDetailsOfUser;
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
}