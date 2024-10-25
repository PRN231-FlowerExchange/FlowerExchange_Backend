using Application.Conversation.DTOs;
using AutoMapper;
using Domain.Commons.BaseRepositories;
using Domain.Repository;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Conversation.Queries.GetConversationById
{
    public class GetConversationByIdQuery : IRequest<ConversationDTO>
    {
        public Guid conversationId { get; set; }
        public GetConversationByIdQuery(Guid ConversationId)
        {
            conversationId = ConversationId;
        }
    }

    public class GetConversationByIdQueryQueryHandler : IRequestHandler<GetConversationByIdQuery, ConversationDTO>
    {
        private IMapper _mapper;
        private IConversationRepository _conversationRepository;
        private IUserConversationRepository _userConversationRepository;
        private IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;

        public async Task<ConversationDTO> Handle(GetConversationByIdQuery request, CancellationToken cancellationToken)
        {
            var conversation = await _conversationRepository.GetConversationByIdAsync(request.conversationId);
            var response = _mapper.Map<ConversationDTO>(conversation);
            return response;
        }
    }
}
