using AutoMapper;
using BAL.Dtos;
using DAL.Models;

namespace BAL.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            UserRatingConfiguration();
            
            UserClientConfiguration();
            
            UserDriverConfiguration();
        }

        private void UserRatingConfiguration()
        {
            CreateMap<UserRating, UserRatingDto>();
        }

        private void UserClientConfiguration()
        {
            CreateMap<User, ClientReadDto>()
               .ForMember(dest => dest.UserRatingDtos, opt => opt.MapFrom(src => src.UserRatings));

            CreateMap<ClientReadDto, User>();

            CreateMap<User, ClientCreateDto>();

            CreateMap<ClientCreateDto, User>();

            CreateMap<User, UserTokenDto>();
        }

        private void UserDriverConfiguration()
        {
            CreateMap<DriverCreateDto, User>();

            CreateMap<DriverInfo, DriverInfoDto>();

            CreateMap<User, DriverReadDto>()
                .ForMember(dest => dest.DriverInfoDto, opt => opt.MapFrom(src => src.DriverInfo))
                .ForMember(dest => dest.UserRatingDtos, opt => opt.MapFrom(src => src.UserRatings));
        }
    }
}
