using Application.UserStore.Command.CreateUserStore;
using Application.UserStore.DTOs;
using Application.UserStore.Queries.GetStoreInDetails;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/store")]
    public class StoreController : APIControllerBase
    {

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateStore([FromBody] CreateUserStoreCommand command)
        {
            Guid storeId = await this.Mediator.Send(command);
            return Ok(new { userId = HttpContext.User.Identity.GetUserId(), storeId = storeId });

        }

        [HttpGet("id/{storeId}")]
        public async Task<StoreViewInDetailsDTO> GetStoreById([FromRoute] Guid storeId)
        {
            StoreViewInDetailsDTO store = await this.Mediator.Send(new GetStoreInDetailsByIdQuery() { StoreID = storeId });
            return store;

        }

        [HttpGet("slug/{slug}")]
        public async Task<StoreViewInDetailsDTO> GetStoreBySlug([FromRoute] string slug)
        {
            StoreViewInDetailsDTO store = await this.Mediator.Send(new GetStoreInDetailsBySlugQuery() { Slug = slug });
            return store;

        }
    }
}
