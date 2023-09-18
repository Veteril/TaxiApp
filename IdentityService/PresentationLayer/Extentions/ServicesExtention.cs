using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using BAL.Profiles;
using BAL.Services;
using Microsoft.AspNetCore.Server.IIS.Core;
using System.Runtime.CompilerServices;

namespace PresentationLayer.Extentions
{
    public static class ServicesExtention
    {
        public static IServiceCollection AddServices
            (this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IClientRepo, ClientRepo>();
            services.AddScoped<ClientsService>();
            services.AddAutoMapper(typeof(UserProfile).Assembly);
            services.AddDbContext<UserDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: "InMemoryDatabase"));

            //SeedData(services);
            return services;
        }

        private static void SeedData(this IServiceCollection services)
        {
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();

                if (!dbContext.Clients.Any())
                {
                    Console.WriteLine("--> Seeding Data...");
                    dbContext.Clients.AddRange(
                            new Client() { Email = "hskfhkhgj", Id = 1, PasswordHash = "sdfghjkl;;lk", Phone = "+3754864335", Username = "Holera", Rating = 4.5F },
                            new Client() { Email = "fsjflksj", Id = 2, PasswordHash = "sdfghjksgsgsg;;lk", Phone = "+3724864335", Username = "Freddy", Rating = 4.5F },
                            new Client() { Email = "jlksfjlksjf", Id = 3, PasswordHash = "sdfghjkl;hjdsjfh;lk", Phone = "+3784864335", Username = "Fazbear", Rating = 4.5F }
                        );
                    dbContext.SaveChanges();
                }
                else
                {
                    Console.WriteLine("--> Data already exists...");
                }
            }
        }
    }
}
