using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> opt) : base(opt) { }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Driver> Drivers { get; set; }

        public DbSet<ClientRating> ClientRatings { get; set; }

        public DbSet<DriverRating> DriverRatings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ClientRatingConfiguration(modelBuilder);
            
            DriverRatingConfiguration(modelBuilder);
            
            ClientConfiguration(modelBuilder);
            
            DriverConfiguration(modelBuilder);
        }

        private void ClientRatingConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientRating>()
                .HasOne(cr => cr.Client)
                .WithMany(c => c.ClientRatings)
                .HasForeignKey(cr => cr.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void DriverRatingConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DriverRating>()
                .HasOne(dr => dr.Driver)
                .WithMany(d => d.DriverRatings)
                .HasForeignKey(dr => dr.DriverId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ClientConfiguration(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Client>()
                .HasKey(c => c.Id);
        }

        private void DriverConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Driver>()
                .HasKey(d => d.Id);
        }
    }
}
