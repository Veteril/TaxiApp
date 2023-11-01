using AutoMapper;
using OrdersService.Application.Dtos;
using OrdersService.Application.Interfaces;
using OrdersService.Domain.Models;

namespace OrdersService.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;

        public OrderService(
            IOrdersRepository ordersRepository,
            IMapper mapper,
            IMessageBusClient messageBusClient)
        {
            _ordersRepository = ordersRepository;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
        }

        public async Task<OrderNotificationDto> CreateOrderAsync(OrderCreateDto orderCreateDto, string clientId)
        {
            var order = _mapper.Map<Order>(orderCreateDto);
            order.ClientId = clientId;

            order = await _ordersRepository.CreateOrderAsync(order);

            var orderNotificationDto = _mapper.Map<OrderNotificationDto>(order);
            return orderNotificationDto;
        }

        public async Task DeclineOrderAsync(string orderId, string role)
        {
            var order = await _ordersRepository.GetOrderByIdAsync(orderId);
            
            switch(role)
            {
                case "Client":
                    order.Status = 2;
                    break;
                case "Driver":
                    order.Status = 3;
                    break;
            }

            _ordersRepository.UpdateOrder(order.Id, order);
        }

        public async Task AcceptOrderAsync(string orderId, string driverId)
        {
            var order = await _ordersRepository.GetOrderByIdAsync(orderId);

            order.Status = 1;
            order.DriverId = driverId;

            _ordersRepository.UpdateOrder(order.Id, order);
        }

        public async Task CompleteOrderAsync(string orderId)
        {
            var order = await _ordersRepository.GetOrderByIdAsync(orderId);
            order.Status = 4;

            _ordersRepository.UpdateOrder(order.Id, order);
        }

        public async Task SetMarkToOrderAsync(string orderId, int mark, string role)
        {
            var order = await _ordersRepository.GetOrderByIdAsync(orderId);

            var markDto = GetMessageForMessageBus(role, order, mark);

            try
            {
                _messageBusClient.PublishNewOrderMark(markDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not send Async {ex.Message}");
            }
        }

        private OrderMarkPublishedDto GetMessageForMessageBus(string role, Order order, int mark)
        {
            var markDto = new OrderMarkPublishedDto();
            switch (role)
            {
                case "Client":
                    order.DriverMark = mark;
                    markDto.Mark = mark;
                    markDto.UserId = order.DriverId;
                    break;
                case "Driver":
                    order.ClientMark = mark;
                    markDto.Mark = mark;
                    markDto.UserId = order.ClientId;
                    break;
            }

            markDto.Event = "OrderMarkPublished";
            _ordersRepository.UpdateOrder(order.Id, order);
                
            return markDto;
        }

    }
}
