using Microsoft.Extensions.Configuration;
using OrdersService.Application.Dtos;
using OrdersService.Application.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace OrdersService.Infrastructure.MessageBus
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IChannel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;

            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMqHost"],
                Port = int.Parse(_configuration["RabbitMqPort"])
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateChannel();

            _channel.ExchangeDeclare(exchange: "OrderMark", type: ExchangeType.Fanout);
        }

        public void PublishNewOrderMark(OrderMarkPublishedDto markDto)
        {
            var message = JsonSerializer.Serialize(markDto);

            SendMessage(message);
        }

        public void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                exchange: "OrderMark",
                routingKey: "",
                body: body);
        }
    }
}
