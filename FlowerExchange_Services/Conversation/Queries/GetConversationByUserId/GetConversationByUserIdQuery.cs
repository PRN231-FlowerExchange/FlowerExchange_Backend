using Application.Conversation.DTOs;
using AutoMapper;
using Domain.Commons.BaseRepositories;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Conversation.Queries.GetConversationByUserIdQuery
{
    public class GetConversationByUserIdQuery : IRequest<List<ConversationDetailDTO>>
    {
        public Guid userId { get; set; }
        public GetConversationByUserIdQuery(Guid UserId)
        {
            userId = UserId;
        }
    }

    public class GetConversationByUserIdQueryHandler : IRequestHandler<GetConversationByUserIdQuery, List<ConversationDetailDTO>>
    {
        private IMapper _mapper;
        private IConversationRepository _conversationRepository;
        private IUserConversationRepository _userConversationRepository;
        private IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;
        private ILogger<GetConversationByUserIdQueryHandler> _logger;

        public GetConversationByUserIdQueryHandler(IConversationRepository conversationRepository, IUserConversationRepository userConversationRepository, ILogger<GetConversationByUserIdQueryHandler> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _conversationRepository = conversationRepository;
            _userConversationRepository = userConversationRepository;
        }

        public async Task<List<ConversationDetailDTO>> Handle(GetConversationByUserIdQuery request, CancellationToken cancellationToken)
        {
            var conversation = await _conversationRepository.GetConversationsByUserIdAsync(request.userId);
            var response = _mapper.Map<List<ConversationDetailDTO>>(conversation);
            return response;
        }
    }
}
