
﻿using Application.PostFlower.Queries.GetPost;
using Application.PostFlower.Commands.UpdatePostCommand;
using Application.PostFlower.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.PostFlower.Queries.GetPostService;
using Domain.Entities;
using Application.PostFlower.Commands.AddServiceToPostCommand;
﻿using Application.Post.Queries.GetDetailPost;
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

        //[HttpPost(Name = "post")]
        //public async Task<UpdatePostDTO> UpdatePost([FromBody] UpdatePostCommand command)
        //{
        //    return await Mediator.Send(command);
        //}

        [HttpPut(Name = "update-post")]
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
