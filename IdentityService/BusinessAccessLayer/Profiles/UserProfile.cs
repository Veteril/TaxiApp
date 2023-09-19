using AutoMapper;
using BAL.Dtos;
using DAL.Models;

namespace BAL.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            ClientRatingConfiguration();
            
            DriverRatingConfiguration();
            
            ClientConfiguration();
            
            DriverConfiguration();
        }

        private void DriverRatingConfiguration()
        {
            CreateMap<DriverRating, DriverRatingDto>();   
        }

        private void ClientRatingConfiguration()
        {
            CreateMap<ClientRating, ClientRatingDto>();
        }

        private void ClientConfiguration()
        {
            CreateMap<Client, ClientReadDto>()
               .ForMember(dest => dest.clientRatingsDtos, opt => opt.MapFrom(src => src.ClientRatings));
            
            CreateMap<ClientReadDto, Client>();
            
            CreateMap<Client, ClientCreateDto>();
            
            CreateMap<ClientCreateDto, Client>();
        }

        private void DriverConfiguration()
        {
            CreateMap<Driver, DriverReadDto>()
                .ForMember(dest => dest.driverRatingDtos, opt => opt.MapFrom(src => src.DriverRatings));
            
            CreateMap<DriverReadDto, Driver>();

            CreateMap<Client, ClientCreateDto>();

            CreateMap<DriverCreateDto, Driver>();
        }
    }
}
