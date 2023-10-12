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
using FluentValidation;
using BAL.Validators;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PresentationLayer.Extentions
{
    public static class ServicesExtention
    {
        public static IServiceCollection AddServices
            (this IServiceCollection services, IConfiguration config, IWebHostEnvironment environment)
        {
            ConfigureRepositories(services);

            ConfigureAuthentication(services, config);

            ConfigureServices(services);

            ConfigureMapper(services);

            ConfigureDatabaseContext(services, environment, config);


            return services;
        }

        private static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IClientRepository, ClientRepository>();
            
            services.AddScoped<IDriverRepository, DriverRepository>();
        }

        private static void ConfigureServices (this IServiceCollection services) 
        {
            services.AddScoped<DriversService>();
            
            services.AddScoped<ClientsService>();

            services.AddScoped<TokenService>();

            services.AddScoped<HashService>();

            services.AddSignalR();

            services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();
        }

        private static void ConfigureMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserProfile).Assembly);
        }

        private static void ConfigureDatabaseContext(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration config)
        {
            if (environment.IsDevelopment()) 
            {
                Console.WriteLine("Using Local connection");
                services.AddDbContext<UserDbContext>(opt =>
                    opt.UseSqlServer(config.GetConnectionString("IdentityConnectionLocal")));
            }
            else 
            {
                Console.WriteLine("Using K8S connection");
                services.AddDbContext<UserDbContext>(opt =>
                    opt.UseSqlServer(config.GetConnectionString("IdentityConnectionK8S")));
            }
        }

        private static void ConfigureAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<JwtSettings>(config.GetSection(JwtSettings.SectionName));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    var authenticationOptions = serviceProvider.GetRequiredService<IOptions<JwtSettings>>().Value;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationOptions.Secret)),
                        ValidateIssuer = true,
                        ValidIssuer = authenticationOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = authenticationOptions.Audience,
                        ValidateLifetime = true
                    };
                });
        }
    }
}
