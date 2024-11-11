using Application.Order.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/flower-order")]
public class FlowerOrderController : APIControllerBase
{
    private readonly ILogger<FlowerOrderController> _logger;

    public FlowerOrderController(ILogger<FlowerOrderController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{userId}/user")]
    public async Task<IActionResult> GetFlowerOrdersOfUser(Guid userId)
    {
        try
        {
            var response = await Mediator.Send(new GetHistoryFlowerOrderOfUserQuery(userId));
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}