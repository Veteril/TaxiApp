using AutoMapper;
using OrdersService.Application.Dtos;
using OrdersService.Domain.Models;

namespace OrdersService.Application.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile() 
        {
            CreateMap<OrderCreateDto, Order>();

            CreateMap<Order, OrderNotificationDto>();
        }
    }
}
