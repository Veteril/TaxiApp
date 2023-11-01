using AutoMapper;
using BAL.Dtos;
using BAL.Dtos.RatingDtos;
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

            CreateMap<UserRatingPublishedDto, UserRating>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Mark, opt => opt.MapFrom(src => src.Mark));
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
