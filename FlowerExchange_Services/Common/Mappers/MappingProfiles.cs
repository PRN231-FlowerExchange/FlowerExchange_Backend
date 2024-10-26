
using Application.Category.DTOs;
using Application.PostFlower.DTOs;
using Application.UserIdentity.DTOs;
using Application.UserStore.DTOs;
using AutoMapper;
using Domain.Entities;
using DomainEntities = Domain.Entities;

namespace Application.Common.Mappers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region UserMappings
            CreateMap<User, CurrentUserModel>();
            #endregion

            #region PostMappings
            CreateMap<CreatePostDTO, DomainEntities.Post>();
            CreateMap<Domain.Entities.Post, PostDTO>().ReverseMap();
            CreateMap<Domain.Entities.Post, AllPostDTO>().ReverseMap();
            #endregion

            #region FlowerMappings
            CreateMap<FlowerDTO, Flower>().ReverseMap();
            #endregion

            #region StoreMappings
            CreateMap<StoreCreateDTO, Store>();
            CreateMap<Store, StoreDTO>().ReverseMap();
            CreateMap<Store, StoreViewInDetailsDTO>();
            #endregion

            #region CategoryMappings
            CreateMap<Domain.Entities.Category, CategoryDTO>().ReverseMap();
            #endregion

        }
    }
}
