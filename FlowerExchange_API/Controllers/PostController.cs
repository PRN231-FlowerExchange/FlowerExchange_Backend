using Application.Post.Queries.GetDetailPost;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Application.Post.Commands.CreatePost;
using Application.Post.DTOs;
using MediatR;
using System.Threading.Tasks;
using DomainEntities = Domain.Entities;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : APIControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private readonly IMediator _mediator;
        public PostController(ILogger<PostController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailPost(Guid id)
        {
            var response = await Mediator.Send(new GetDetailPostQuery(id));
            return Ok(response);
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

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDTO createPostDTO)
        {
            var command = new CreatePostCommand(createPostDTO);
            Guid result = await _mediator.Send(command);
            return Ok(new { postId = result });
        }
    }
}
