using AutoMapper;
using BAL.Dtos;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<UserRating, ClientRatingDto>();

            CreateMap<Client, ClientReadDto>()
                .ForMember(dest => dest.clientRatings, opt => opt.MapFrom(src => src.UserRatings));
            CreateMap<ClientReadDto, Client>();
            CreateMap<Client, ClientCreateDto>();
            CreateMap<ClientCreateDto, Client>();
            CreateMap<Driver, DriverReadDto>();
            CreateMap<DriverCreateDto, Driver>();

        }
    }
}
