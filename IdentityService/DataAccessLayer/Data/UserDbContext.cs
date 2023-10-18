using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> opt) : base(opt) {}

        public DbSet<User> Users { get; set; }

        public DbSet<UserRating> UserRatings { get; set; }

        public DbSet<DriverInfo> DriversInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            UserRatingConfiguration(modelBuilder);
            
            UserConfiguration(modelBuilder);
            
            DriverInfoConfiguration(modelBuilder);
        }

        private void UserRatingConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRating>()
                .HasOne(cr => cr.User)
                .WithMany(c => c.UserRatings)
                .HasForeignKey(cr => cr.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void UserConfiguration(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<User>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<User>()
                .HasOne(u => u.DriverInfo)
                .WithOne(di => di.User)
                .HasForeignKey<User>(di => di.DriverInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .Property(u => u.DriverInfoId)
                .IsRequired(false);
        }

        private void DriverInfoConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DriverInfo>()
                .HasKey(di => di.Id);

            modelBuilder.Entity<DriverInfo>()
                .HasOne(di => di.User)
                .WithOne(u => u.DriverInfo)
                .HasForeignKey<DriverInfo>(di => di.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
