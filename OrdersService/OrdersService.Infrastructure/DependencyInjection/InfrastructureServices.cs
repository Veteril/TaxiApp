using Microsoft.Extensions.DependencyInjection;
using OrdersService.Application.Interfaces;
using OrdersService.Infrastructure.MessageBus;
using OrdersService.Infrastructure.Repositories;

namespace OrdersService.Infrastructure.DependencyInjection
{
    public static class InfrastructureServices
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services) 
        {
            services.AddScoped<IOrdersRepository, OrdersRepository>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();

            return services;
        }
    }
}