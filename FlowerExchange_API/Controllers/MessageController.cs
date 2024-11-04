using Application.Conversation.DTOs;
using Application.Conversation.Queries.GetConversationByUserIdQuery;
using Application.Message.Commands.SendMessage;
using Application.Message.DTOs;
using Application.Message.Queries.GetMessagesByConversationId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : APIControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly IMediator _mediator;
        private readonly IHubContext<ChatHub> _chatHubContext;
        public MessageController(ILogger<MessageController> logger, IMediator mediator, IHubContext<ChatHub> chatHubContext)
        {
            _logger = logger;
            _mediator = mediator;
            _chatHubContext = chatHubContext;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] MessageDTO messageDTO)
        {
            var messageId = await Mediator.Send(new SendMessageCommand(messageDTO));
            await _chatHubContext.Clients.Group(messageDTO.ConversationId.ToString())
                .SendAsync("ReceiveMessage", messageDTO);
            return Ok(messageId);
        }

        [HttpGet("/thread/{conversationId}")]
        public async Task<IActionResult> GetMessages(Guid conversationId)
        {
            var response = await Mediator.Send(new GetMessagesByConversationIdQuery(conversationId));
            return Ok(response);
        }
    }
}
