using Application.Conversation.DTOs;
using Application.Conversation.Queries.GetConversationByUserIdQuery;
using Application.Post.Commands.CreatePost;
using Application.Post.DTOs;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConversationController : APIControllerBase
    {
        private readonly ILogger<ConversationController> _logger;
        private readonly IMediator _mediator;
        public ConversationController(ILogger<ConversationController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        //[HttpPost("/{id}")]
        //public async Task<IActionResult> CreateConversation([FromBody] ConversationDTO conversationDTO)
        //{
        //    var response = await Mediator.Send(new CreateConversationCommand(conversationDTO));
        //    return Ok(response);
        //}

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserConversations(Guid userId)
        {
            var query = await Mediator.Send(new GetConversationByUserIdQuery(userId));
            return Ok(query);
        }
    }
}
