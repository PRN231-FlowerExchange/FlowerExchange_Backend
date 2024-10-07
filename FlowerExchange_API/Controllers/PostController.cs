using Application.PostFlower.Queries.GetPost;
using Application.PostFlower.Commands.UpdatePostCommand;
using Application.PostFlower.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.PostFlower.Queries.GetPostService;
using Domain.Entities;
using Application.PostFlower.Commands.AddServiceToPostCommand;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/post")]
    public class PostController : APIControllerBase
    {
        // Changed route for UpdatePost to avoid conflict
        [HttpPut("update")]
        public async Task<PostUpdateDTO> UpdatePost([FromBody] UpdatePostCommand command) => await Mediator.Send(command);

        // AddPostToService endpoint should have a unique name
        [HttpPut("add-service")]
        public async Task<PostViewDTO> AddServiceToPost([FromBody] AddServiceToPostCommand command)
        {
            return await Mediator.Send(command);
        }

        // Kept GetPost as POST with its own path
        [HttpPost("get")]
        public async Task<List<PostViewDTO>> GetPost([FromBody] GetPostQuery query) => await Mediator.Send(query);
    }
}
