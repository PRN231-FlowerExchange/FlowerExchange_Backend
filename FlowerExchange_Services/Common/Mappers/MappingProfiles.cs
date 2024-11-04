

using Application.Conversation.DTOs;
using Application.Message.DTOs;
using Application.Category.DTOs;
using Application.PostFlower.DTOs;
using Application.UserIdentity.DTOs;
using Application.UserStore.DTOs;
using Application.Wallet.DTOs;
using AutoMapper;
using Domain.Entities;
using Microsoft.OpenApi.Extensions;
using DomainEntities = Domain.Entities;

namespace Application.Common.Mappers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, CurrentUserModel>();
            CreateMap<CreatePostDTO, DomainEntities.Post>().ReverseMap();
            CreateMap<Domain.Entities.Post, PostDTO>().ReverseMap();
            CreateMap<Domain.Entities.Post, AllPostDTO>().ReverseMap();
            CreateMap<Domain.Entities.Conversation, ConversationDTO>().ReverseMap();
            CreateMap<Domain.Entities.Message, MessageDTO>().ReverseMap();
            CreateMap<Domain.Entities.Message, Message.DTOs.MessageThreadDTO>().ReverseMap();
            CreateMap<Domain.Entities.Message, MessageConversationDTO>().ReverseMap();
            CreateMap<Domain.Entities.Conversation, ConversationDetailDTO>().ReverseMap();
            CreateMap<Domain.Entities.UserConversation, UserConversationDetailDTO>().ReverseMap();
            CreateMap<Domain.Entities.User, UserMessageDTO>().ReverseMap();
            CreateMap<FlowerDTO, Flower>().ReverseMap();
            CreateMap<StoreCreateDTO, Store>();
            CreateMap<Store, StoreDTO>().ReverseMap();
            CreateMap<Store, StoreViewInDetailsDTO>();
            CreateMap<Domain.Entities.Category, CategoryDTO>().ReverseMap();
            CreateMap<DomainEntities.Wallet, WalletDetailsResponse>()
                .ForMember(
                    dest => dest.Currency, 
                    opt => opt.MapFrom(src => src.Currency.GetDisplayName())
                    );
        }
    }
}
