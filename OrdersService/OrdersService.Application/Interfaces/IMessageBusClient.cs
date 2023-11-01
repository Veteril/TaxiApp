using OrdersService.Application.Dtos;

namespace OrdersService.Application.Interfaces
{
    public interface IMessageBusClient
    {
        void PublishNewOrderMark(OrderMarkPublishedDto markDto);
    }
}