using Application.Message.DTOs;
using AutoMapper;
using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Message.Commands.SendMessage
{
    public class SendMessageCommand : IRequest<MessageDTO>
    {
        public MessageDTO MessageDTO { get; set; }

        public SendMessageCommand(MessageDTO messageDTO) 
        {
            MessageDTO = messageDTO;
        }
    }
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, MessageDTO>
    {
        private IMapper _mapper;
        private IMessageRepository _messageRepository;
        private IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;

        public SendMessageCommandHandler(IMapper mapper, IMessageRepository messageRepository, IUnitOfWork<FlowerExchangeDbContext> unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _messageRepository = messageRepository;
        }

        public async Task<MessageDTO> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var message = new Domain.Entities.Message
            {
                ConversationId = request.MessageDTO.ConversationId,
                SenderId = request.MessageDTO.SenderId,
                Content = request.MessageDTO.Content,
                SentAt = DateTime.UtcNow
            };

            //await _messageRepository.AddMessageAsync(message);
            //await _messageRepository.SaveAsync();

            await _messageRepository.SendMessageAsync(request.MessageDTO.SenderId, request.MessageDTO.RecipientId, request.MessageDTO.Content);

            var messageDto = _mapper.Map<MessageDTO>(message);

            return messageDto;
        }
    }

}
