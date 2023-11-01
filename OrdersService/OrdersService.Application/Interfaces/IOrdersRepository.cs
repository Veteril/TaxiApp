using OrdersService.Domain.Models;

namespace OrdersService.Application.Interfaces
{
    public interface IOrdersRepository
    {
        Task<Order> CreateOrderAsync(Order order);

        Task<Order> GetOrderByIdAsync(string id);

        Task<List<Order>> GetOrdersAsync();

        void DeleteOrder(string id);

        void UpdateOrder(string id, Order order);
    }
}
