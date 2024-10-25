using Application.Conversation.DTOs;
using Application.Conversation.Queries.GetConversationByUserIdQuery;
using Application.Message.Commands.SendMessage;
using Application.Message.DTOs;
using Application.Message.Queries.GetMessagesByConversationId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : APIControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly IMediator _mediator;
        public MessageController(ILogger<MessageController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] MessageDTO messageDTO)
        {
            var messageId = await Mediator.Send(new SendMessageCommand(messageDTO));
            return Ok(messageId);
        }

        [HttpGet("/thread/{userId}")]
        public async Task<IActionResult> GetMessages(Guid userId)
        {
            var response = await Mediator.Send(new GetMessagesByConversationIdQuery(userId));
            return Ok(response);
        }
    }
}
