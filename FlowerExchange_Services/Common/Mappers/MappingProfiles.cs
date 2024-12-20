

using Application.Conversation.DTOs;
using Application.Message.DTOs;
using Application.Category.DTOs;
using Application.Order.DTOs;
using Application.PostFlower.DTOs;
using Application.UserIdentity.DTOs;
using Application.UserStore.DTOs;
using Application.UserWallet.DTOs;
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
            CreateMap<Domain.Entities.User, SellerDTO>().ReverseMap();
            CreateMap<Domain.Entities.PostCategory, PostCategoryDTO>().ReverseMap();
            CreateMap<Domain.Entities.PostCategory, CategoryDetailDTO>().ReverseMap();
            CreateMap<Domain.Entities.Category, CategoryDetailDTO>().ReverseMap();
            CreateMap<DomainEntities.Wallet, WalletDetailsResponse>()
                .ForMember(
                    dest => dest.Currency, 
                    opt => opt.MapFrom(src => src.Currency.GetDisplayName())
                    );
            CreateMap<Domain.Entities.Transaction, WalletTransactionListResponse>()
                .ForMember(
                    dest => dest.Status, 
                    opt 
                        => opt.MapFrom(src => src.Status.GetDisplayName())
                    )
                .ForMember(
                    dest => dest.Type, 
                    opt 
                        => opt.MapFrom(src => src.Type.GetDisplayName())
                );
            CreateMap<WalletTransaction, WalletTransactionOfUserListResponse>()
                .ForMember(
                    dest => dest.Id, 
                    opt 
                        => opt.MapFrom(src => src.Transaction.Id)
                        )
                .ForMember(
                    dest => dest.Amount, 
                    opt 
                        => opt.MapFrom(src => src.Transaction.Amount)
                )
                .ForMember(
                    dest => dest.Type, 
                    opt 
                        => opt.MapFrom(src => src.Transaction.Type.GetDisplayName())
                        )
                .ForMember(
                    dest => dest.Direction, 
                    opt 
                        => opt.MapFrom(src => src.Type.GetDisplayName())
                )
                .ForMember(
                    dest => dest.Status, 
                    opt 
                        => opt.MapFrom(src => src.Transaction.Status.GetDisplayName())
                )
                .ForMember(
                    dest => dest.FromWallet, 
                    opt 
                        => opt.MapFrom(src => src.Transaction.FromWallet)
                )
                .ForMember(
                    dest => dest.ToWallet, 
                    opt 
                        => opt.MapFrom(src => src.Transaction.ToWallet)
                )
                .ForMember(
                    dest => dest.CreateAt, 
                    opt 
                        => opt.MapFrom(src => ((DateTimeOffset)src.CreatedAt).ToOffset(TimeSpan.FromHours(7)).ToString()))
                ;

            CreateMap<ServiceOrder, ServiceOrderOfUserWalletTransaction>()
                .ForMember(dest => dest.Status,
                    opt 
                        => opt.MapFrom(src => src.Status.GetDisplayName()))
                .ForMember(
                    dest => dest.BuyerName, 
                    opt 
                        => opt.MapFrom(src => src.Buyer.Fullname)
                )
                ;
            
            CreateMap<FlowerOrder, FlowerOrderOfUserWalletTransaction>()
                .ForMember(dest => dest.Status,
                    opt 
                        => opt.MapFrom(src => src.Status.GetDisplayName()))
                .ForMember(
                    dest => dest.BuyerName, 
                    opt 
                        => opt.MapFrom(src => src.Buyer.Fullname)
                )
                .ForMember(
                    dest => dest.SellerName, 
                    opt 
                        => opt.MapFrom(src => src.Seller.Fullname)
                )
                ;

            CreateMap<WalletTransaction, WalletTransactionOfUserDetailsResponse>()
                .ForMember(
                    dest => dest.Id, 
                    opt 
                        => opt.MapFrom(src => src.Transaction.Id)
                )
                .ForMember(
                    dest => dest.Amount, 
                    opt 
                        => opt.MapFrom(src => src.Transaction.Amount)
                )
                .ForMember(
                    dest => dest.Type, 
                    opt 
                        => opt.MapFrom(src => src.Transaction.Type.GetDisplayName())
                )
                .ForMember(
                    dest => dest.Direction, 
                    opt 
                        => opt.MapFrom(src => src.Type.GetDisplayName())
                )
                .ForMember(
                    dest => dest.Status, 
                    opt 
                        => opt.MapFrom(src => src.Transaction.Status.GetDisplayName())
                )
                .ForMember(
                    dest => dest.FromWallet, 
                    opt 
                        => opt.MapFrom(src => src.Transaction.FromWallet)
                )
                .ForMember(
                    dest => dest.ToWallet, 
                    opt 
                        => opt.MapFrom(src => src.Transaction.ToWallet)
                )
                .ForMember(
                    dest => dest.ServiceOrder, 
                    opt 
                        => opt.MapFrom(src => src.Transaction.ServiceOrder)
                        )
                .ForMember(
                    dest => dest.FlowerOrder, 
                    opt 
                        => opt.MapFrom(src => src.Transaction.FlowerOrder)
                        )
                .ForMember(
                    dest => dest.CreateAt, 
                    opt 
                        => opt.MapFrom(src => src.CreatedAt.ToString())
                        )
                ;
            CreateMap<FlowerOrder, FlowerOrderHistoryListResponse>()
                .ForMember(
                    dest => dest.Status,
                    opt
                        => opt.MapFrom(src => src.Status.GetDisplayName())
                )
                .ForMember(
                    dest => dest.CreatedAt,
                    opt
                        => opt.MapFrom(src => ((DateTimeOffset)src.CreatedAt).ToOffset(TimeSpan.FromHours(7)).ToString())
                );
            
            CreateMap<Flower, FlowerForFlowerOrderHistoryList>()
                .ForMember(
                    dest => dest.Currency,
                    opt
                        => opt.MapFrom(src => src.Currency.GetDisplayName())
                );
        }
    }
}
