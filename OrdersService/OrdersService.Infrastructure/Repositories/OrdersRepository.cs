using MongoDB.Driver;
using OrdersService.Application.Interfaces;
using OrdersService.Domain.Models;
using OrdersService.Infrastructure.Data;

namespace OrdersService.Infrastructure.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly IMongoCollection<Order> _orders;

        public OrdersRepository(IMongoClient mongoClient, DataContext settings)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _orders = database.GetCollection<Order>(settings.CollectionName);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _orders.InsertOneAsync(order);
            return order;
        }

        public async Task<Order> GetOrderByIdAsync(string id)
        {
            var order = await _orders.Find(or => or.Id == id).FirstOrDefaultAsync();
            return order;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            var orders = await _orders.Find(or => true).ToListAsync();
            return orders;
        }

        public void DeleteOrder(string id)
        {
            _orders.DeleteOne(or => or.Id == id);
        }

        public void UpdateOrder(string id, Order order)
        {
            _orders.ReplaceOne(or => or.Id == id, order);
        }
    }
}
