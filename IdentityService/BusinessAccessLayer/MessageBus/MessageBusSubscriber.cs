using BAL.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.MessageBus
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessorService _eventProcessorService;
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private string _queueName;

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessorService eventProcessorService)
        {
            _configuration = configuration;
            _eventProcessorService = eventProcessorService;

            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMqHost"],
                Port = int.Parse(_configuration["RabbitMqPort"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateChannel();

                _channel.ExchangeDeclare(exchange: "OrderMark", type: ExchangeType.Fanout);

                _queueName = "OrderMarkQueue";
                _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                _channel.QueueBind(_queueName, "OrderMark", "");

                Console.WriteLine("Connected to RabbitMq successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cant connect to RabbitMq {ex.Message}");
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine("Received: " + message);
                _eventProcessorService.ProcessEvent(message);
            };

            _channel.BasicConsume(_queueName, true, consumer);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}
