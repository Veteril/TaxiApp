using Microsoft.Extensions.DependencyInjection;
using OrdersService.Application.Interfaces;
using OrdersService.Application.Profiles;
using OrdersService.Application.Services;

namespace OrdersService.Application.DependencyInjection
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();

            services.AddAutoMapper(typeof(OrderProfile).Assembly);

            return services;
        }
    }
}
