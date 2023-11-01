using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using OrdersService.Application.Authentication;
using OrdersService.Application.DependencyInjection;
using OrdersService.Infrastructure.Data;
using OrdersService.Infrastructure.DependencyInjection;
using System.Text;

namespace OrdersService.API.Extentions
{
    public static class ServicesExtention
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config) 
        {
            services.AddInfrastructure();
            services.AddApplication();
            services.AddSwagger();

            services.AddSignalR();

            services.Configure<DataContext>(config.GetSection(nameof(DataContext)));

            services.AddSingleton(sp => sp.GetRequiredService<IOptions<DataContext>>().Value);

            services.AddSingleton<IMongoClient>(s => new MongoClient(config.GetValue<string>("DataContext:ConnectionString")));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowClient",
                    builder => builder.WithOrigins("http://localhost:3000")
                                     .AllowAnyHeader()
                                     .AllowAnyMethod()
                                     .AllowCredentials());
            });

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

            return services;
        }

        private static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "OrdersService",
                    Description = "OrdersService Swagger",
                    Contact = new OpenApiContact
                    {
                        Name = "Stepan Shandrakov",
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
            });
            });

            return services;
        }
    }
}
