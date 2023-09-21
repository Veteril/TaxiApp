using DAL.Data;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using BAL.Profiles;
using BAL.Services;
using Microsoft.AspNetCore.Server.IIS.Core;
using System.Runtime.CompilerServices;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BAL.Authentication;

namespace PresentationLayer.Extentions
{
    public static class ServicesExtention
    {
        public static IServiceCollection AddServices
            (this IServiceCollection services, IConfiguration config)
        {
            ConfigureRepositories(services);

            ConfigureServices(services, config);

            ConfigureMapper(services);

            ConfigureDatabaseContext(services);

            return services;
        }

        private static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IClientRepository, ClientRepository>();
            
            services.AddScoped<IDriverRepository, DriverRepository>();
        }

        private static void ConfigureServices (this IServiceCollection services, IConfiguration config) 
        {
            services.Configure<JwtSettings>(config.GetSection(JwtSettings.SectionName));

            services.AddScoped<DriversService>();
            
            services.AddScoped<ClientsService>();

            services.AddScoped<TokenService>();
        }

        private static void ConfigureMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserProfile).Assembly);
        }

        private static void ConfigureDatabaseContext(this IServiceCollection services)
        {
            services.AddDbContext<UserDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: "InMemoryDatabase"));
        }
    }
}
