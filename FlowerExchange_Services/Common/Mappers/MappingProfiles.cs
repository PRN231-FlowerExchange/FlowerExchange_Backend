
using Application.Category.DTOs;
using Application.PostFlower.DTOs;
using Application.UserIdentity.DTOs;
using Application.Weather.DTOs;
using AutoMapper;
using Domain.Entities;
using DomainEntities = Domain.Entities;

namespace Application.Common.Mappers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            CreateMap<User, CurrentUserModel>();
            CreateMap<WeatherForecastDTO, WeatherForecast>().ReverseMap();
            CreateMap<Store, StoreDTO>().ReverseMap();
            CreateMap<Domain.Entities.Post, PostDTO>().ReverseMap();
            CreateMap<CreatePostDTO, DomainEntities.Post>();
            CreateMap<FlowerDTO, Flower>().ReverseMap();
            CreateMap<Domain.Entities.Post, AllPostDTO>().ReverseMap();
            CreateMap<Domain.Entities.Category, CategoryDTO>().ReverseMap();
        }
    }
}
