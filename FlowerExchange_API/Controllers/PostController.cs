using Application.Post.Queries.GetDetailPost;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : APIControllerBase
    {
        private readonly ILogger<PostController> _logger;
        public PostController(ILogger<PostController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<Post> GetDetailPost(Guid id)
        {
            return await Mediator.Send(new GetDetailPostQuery(id));

            //if (post == null)
            //{
            //    return NotFound();
            //}

            //return Ok(post);
        }

        [HttpGet("store/{id}")]
        public async Task<IActionResult> GetUserPost(Guid id, [FromQuery] PostParameters postParameters)
        {
            try
            {
                var response = await Mediator.Send(new GetUserPostQuery(id, postParameters));
                var metadata = new
                {
                    response.TotalCount,
                    response.PageSize,
                    response.CurrentPage,
                    response.TotalPages,
                    response.HasNext,
                    response.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

    }
}
