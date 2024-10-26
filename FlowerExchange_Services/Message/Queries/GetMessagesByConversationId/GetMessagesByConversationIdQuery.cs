using Application.Message.DTOs;
using Application.Post.DTOs;
using Application.Post.Queries.GetDetailPost;
using AutoMapper;
using Domain.Exceptions;
using Domain.Models;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Message.Queries.GetMessagesByConversationId
{
    public class GetMessagesByConversationIdQuery : IRequest<List<MessageThreadDTO>>
    {
        public Guid ConversationId { get; set; }
        public GetMessagesByConversationIdQuery(Guid conversationId)
        {
            ConversationId = conversationId;
        }
    }

    public class GetMessagesByConversationIdQueryHandler : IRequestHandler<GetMessagesByConversationIdQuery, List<MessageThreadDTO>>
    {
        private IMessageRepository _messageRepository;

        private readonly ILogger<GetMessagesByConversationIdQueryHandler> _logger;

        private readonly IMapper _mapper;

        public GetMessagesByConversationIdQueryHandler(IMessageRepository messageRepository, ILogger<GetMessagesByConversationIdQueryHandler> logger, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<MessageThreadDTO>> Handle(GetMessagesByConversationIdQuery request, CancellationToken cancellationToken)
        {
            var messages = await _messageRepository.GetMessageThreadAsync(request.ConversationId);
            if (messages == null)
            {
                var errorMessage = $"Message with Conversation Id: {request.ConversationId} was not found.";
                _logger.LogWarning(errorMessage);
                throw new NotFoundException(errorMessage);
            }
            var response = _mapper.Map<List<MessageThreadDTO>>(messages);
            return response;
        }
    }
}
