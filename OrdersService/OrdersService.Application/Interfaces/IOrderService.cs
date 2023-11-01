using OrdersService.Application.Dtos;

namespace OrdersService.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderNotificationDto> CreateOrderAsync(OrderCreateDto orderCreateDto, string clientId);
        
        Task DeclineOrderAsync(string id, string role);
        
        Task AcceptOrderAsync(string orderId, string driverId);
        
        Task SetMarkToOrderAsync(string orderId, int mark, string role);

        Task CompleteOrderAsync(string orderId);
    }
}
