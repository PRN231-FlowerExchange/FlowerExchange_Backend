using Application.PostFlower.Queries.GetPost;
using Application.PostFlower.Commands.UpdatePostCommand;
using Application.PostFlower.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/post")]
    public class PostController : APIControllerBase
    {
        private readonly ILogger<PostController> _logger;

        public PostController(ILogger<PostController> logger)
        {
            _logger = logger;
        }

        //[HttpPost(Name = "post")]
        //public async Task<UpdatePostDTO> UpdatePost([FromBody] UpdatePostCommand command)
        //{
        //    return await Mediator.Send(command);
        //}
        [HttpPut(Name = "update-post")]
        public async Task<PostUpdateDTO> UpdatePost([FromBody] UpdatePostCommand command) => await Mediator.Send(command);

        [HttpPost(Name = "get-post")]
        public async Task<List<PostViewDTO>> GetPost([FromBody] GetPostQuery query) => await Mediator.Send(query);
    }
}
