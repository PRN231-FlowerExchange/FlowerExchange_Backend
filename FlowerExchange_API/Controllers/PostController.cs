using Application.Post.Commands.CreatePost;
using Application.Post.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DomainEntities = Domain.Entities; // Tạo bí danh cho namespace Domain.Entities

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDTO createPostDTO)
        {
            var command = new CreatePostCommand(createPostDTO);
            Guid result = await _mediator.Send(command);
            return Ok(new { postId=result});
        }
    }
}
