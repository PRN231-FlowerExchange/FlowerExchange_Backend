



using Application.Post.DTOs;
using Application.SystemUser.DTOs;
using Application.Weather.Commands.AddWeather;
using Application.Weather.DTOs;
using AutoMapper;
using Domain.Entities;
using DomainEntities = Domain.Entities;

namespace Application.Common.Mappers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() {

            CreateMap<User, UserModel>();
            CreateMap<WeatherForecastDTO, WeatherForecast>().ReverseMap();
            CreateMap<CreatePostDTO, DomainEntities.Post>();
            CreateMap<FlowerDTO, Flower>();
        }
    }
}
